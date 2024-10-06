namespace OwlReadingRoom.Proxy;


/// <summary>
/// Specifies that a method should be executed within a transaction.
/// </summary>
/// <remarks>
/// This attribute can be applied to methods to indicate that they should be executed
/// within a transactional context. The TransactionalProxy uses this attribute to 
/// determine whether to create a new transaction or participate in an existing one
/// when invoking the decorated method.
/// </remarks>
/// 
[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class TransactionalAttribute : Attribute
{
    public bool ReadOnly { get; }

    public TransactionalAttribute(bool readOnly = false)
    {
        ReadOnly = readOnly;
    }
}
