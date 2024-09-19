using System;
namespace OwlReadingRoom.Services.Exceptions;

/// <summary>
/// Exception thrown when a duplicate entry is detected, typically for unique fields like mobile numbers.
/// </summary>
public class DuplicateEntryException : Exception
{
    public DuplicateEntryException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntryException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public DuplicateEntryException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntryException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DuplicateEntryException(string message, Exception innerException)
        : base(message, innerException) { }

    // Optional: Add custom properties if needed
    public string DuplicateField { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateEntryException"/> class with a specified error message and the name of the duplicate field.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="duplicateField">The name of the field that caused the duplicate entry.</param>
    public DuplicateEntryException(string message, string duplicateField)
        : base(message)
    {
        DuplicateField = duplicateField;
    }
}
