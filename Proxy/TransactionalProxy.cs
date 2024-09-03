using OwlReadingRoom.Services.Database;
using System.Diagnostics;
using System.Reflection;

namespace OwlReadingRoom.Proxy;

public class TransactionalProxy<T> : DispatchProxy where T : class
{
    private T _decorated;

    private IDatabaseConnectionService _databaseConnectionService;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        /*var hasTransactionalAttribute = targetMethod.GetCustomAttributes(typeof(TransactionalAttribute), true).Any();*/

        var method = _decorated.GetType().GetMethod(targetMethod.Name);

        var hasTransactionalAttribute = method?.GetCustomAttributes(typeof(TransactionalAttribute), true).Any() ?? false;
        
        if ((hasTransactionalAttribute))
        {
            using (_databaseConnectionService)
            {
                try
                {
                    Debug.WriteLine("INSIDE TRANSACTIONAL BLOCK");
                    
                    _databaseConnectionService.BeginTransaction();

                    var result = targetMethod.Invoke(_decorated, args);
                   
                    _databaseConnectionService.CommitTransaction();

                    Debug.WriteLine("TRANSACTION COMMITTED");
                    return result;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    _databaseConnectionService.RollBack();
                }
            }
            
        }

        return targetMethod.Invoke(_decorated, args);

    }

    public static T CreateProxy(T decorated, IDatabaseConnectionService databaseConnectionService)
    {   
        object proxy = Create<T, TransactionalProxy<T>>();
        ((TransactionalProxy<T>)proxy)._decorated = decorated;
        ((TransactionalProxy<T>)proxy)._databaseConnectionService = databaseConnectionService;
        return (T)proxy;
    }

}
