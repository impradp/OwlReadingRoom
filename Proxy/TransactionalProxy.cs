using OwlReadingRoom.Services.Database;
using System.Diagnostics;
using System.Reflection;

namespace OwlReadingRoom.Proxy;


/// <summary>
/// Represents a proxy class that adds transactional behavior to methods of a decorated class.
/// </summary>
/// <typeparam name="T">The type of the decorated class. Must be a reference type.</typeparam>
public class TransactionalProxy<T> : DispatchProxy where T : class
{
    private T _decorated;

    private IDatabaseConnectionService _databaseConnectionService;

    /// <summary>
    /// Invokes the method on the decorated object, applying transactional behavior if necessary.
    /// </summary>
    /// <param name="targetMethod">The MethodInfo of the method being invoked.</param>
    /// <param name="args">An array of arguments to pass to the method.</param>
    /// <returns>The result of the method invocation.</returns>
    /// <remarks>
    /// This method checks for the presence of a TransactionalAttribute on the target method.
    /// If present, it determines whether to invoke the method in a new transaction or participate in an existing one.
    /// If not present, it invokes the method directly on the decorated object.
    /// </remarks>
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        var method = _decorated.GetType().GetMethod(targetMethod.Name);

        var transactionalAttribute = method?.GetCustomAttributes(typeof(TransactionalAttribute), true).FirstOrDefault() as TransactionalAttribute;
        
        if (transactionalAttribute == null)
            return targetMethod.Invoke(_decorated, args);

        if(_databaseConnectionService.Connection == null)
        {
            return InvokeInNewTransaction(targetMethod, args, transactionalAttribute);
        }

        return _databaseConnectionService.Connection.IsInTransaction
        ? InvokeInExistingTransaction(targetMethod, args)
        : InvokeInNewTransaction(targetMethod, args, transactionalAttribute);
    }

    /// <summary>
    /// Invokes the target method within an existing transaction.
    /// </summary>
    /// <param name="targetMethod">The MethodInfo of the method being invoked.</param>
    /// <param name="args">An array of arguments to pass to the method.</param>
    /// <returns>The result of the method invocation.</returns>
    /// <remarks>
    /// This method is called when there's already an active transaction.
    /// It logs a debug message and then invokes the method on the decorated object.
    /// </remarks>
    private object? InvokeInExistingTransaction(MethodInfo targetMethod, object?[]? args)
    {
        Debug.WriteLine("Participating in existing transaction.");
        return targetMethod.Invoke(_decorated, args);
    }

    /// <summary>
    /// Invokes the target method within a new transaction.
    /// </summary>
    /// <param name="targetMethod">The MethodInfo of the method being invoked.</param>
    /// <param name="args">An array of arguments to pass to the method.</param>
    /// <param name="transactionalAttribute">The TransactionalAttribute applied to the method.</param>
    /// <returns>The result of the method invocation.</returns>
    /// <remarks>
    /// This method creates a new transaction (unless the method is marked as read-only),
    /// invokes the target method, and then commits the transaction if successful.
    /// If an exception occurs, it rolls back the transaction (for non-read-only methods) and re-throws the exception.
    /// </remarks>
    private object? InvokeInNewTransaction(MethodInfo targetMethod, object?[]? args, TransactionalAttribute transactionalAttribute)
    {
        using (_databaseConnectionService)
        {
            try
            {
                if (!transactionalAttribute.ReadOnly)
                {
                    Debug.WriteLine("Initiating a new transaction.");
                    _databaseConnectionService.BeginTransaction();
                }

                var result = targetMethod.Invoke(_decorated, args);

                if (!transactionalAttribute.ReadOnly)
                {
                    Debug.WriteLine("Transaction committed successfully.");
                    _databaseConnectionService.CommitTransaction();
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while performing a transaction: {ex.Message}");
                if (!transactionalAttribute.ReadOnly)
                {
                    Debug.WriteLine("Performing rollback");
                    _databaseConnectionService.RollBack();
                }
                throw ex.InnerException ?? ex; // Re-throw the exception after handling
            }
        }
    }

    /// <summary>
    /// Creates a new instance of the TransactionalProxy for the specified decorated object.
    /// </summary>
    /// <param name="decorated">The object to be decorated with transactional behavior.</param>
    /// <param name="databaseConnectionService">The database connection service to use for managing transactions.</param>
    /// <returns>A proxy instance that adds transactional behavior to the decorated object.</returns>
    /// <remarks>
    /// This method uses the DispatchProxy.Create method to create a new proxy instance,
    /// then initializes it with the decorated object and the database connection service.
    /// </remarks>
    public static T CreateProxy(T decorated, IDatabaseConnectionService databaseConnectionService)
    {
        object proxy = Create<T, TransactionalProxy<T>>();
        ((TransactionalProxy<T>)proxy)._decorated = decorated;
        ((TransactionalProxy<T>)proxy)._databaseConnectionService = databaseConnectionService;
        return (T)proxy;
    }

}
