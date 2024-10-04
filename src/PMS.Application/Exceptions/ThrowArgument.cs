namespace PMS.Application.Exceptions;

public class ThrowArgument
{
    public static void ExceptionIfNull(object argument)
    {
        if (argument == null)
        {
            throw new ArgumentException("Cannot be null", nameof(argument));
        }
    }

    public static void ExceptionIfZero(int id)
    {
        if (id == 0)
            throw new ArgumentException("ID cannot be zero.", nameof(id));
    }
}