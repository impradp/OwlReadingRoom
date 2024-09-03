namespace OwlReadingRoom.Proxy;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class TransactionalAttribute : Attribute
{
}
