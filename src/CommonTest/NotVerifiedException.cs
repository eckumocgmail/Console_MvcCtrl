using System;

/// <summary>
/// Исключение пробрасывает методами проверки кода в случае получения отрицательных результатов
/// </summary>
public class NotVerifiedException: Exception
{
    public NotVerifiedException( Type type, string method, string message): 
        base( $"Класс {type.Name} реалитзует функцию {method} не соответвующим образом: {message} " )
    {        
    }
}