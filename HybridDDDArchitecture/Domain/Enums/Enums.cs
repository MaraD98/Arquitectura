namespace Domain.Enums
{
    /// <summary>
    /// Las enumeraciones deben ir definidas aqui
    /// </summary>

    public class Enums
    {
        /// <summary>
        /// Ejemplo de enumeracion Dummy
        /// </summary>
        public enum DummyValues
        {
            value1,
            value2,
            value3,
        }

        public enum DatabaseType
        {
            MYSQL,
            MARIADB,
            SQLSERVER, 
            MONGODB
        }
    }
}
