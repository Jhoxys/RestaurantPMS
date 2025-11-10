namespace RestaurantPMS.Service
{
    using Dapper;
    using System;
    using System.Data;
    using System.Globalization;

    public class ServiceHelper
    {
        // Define el ID de transacción fijo (puede ser '01' para ventas, '02' para compras, etc.)
        private const string TransactionIdPrefix = "01";

        private readonly DapperContext _context;

        public ServiceHelper(DapperContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Genera un número de transacción único con el formato: 01YYYYMMDDSSSS.
        /// Obtiene el número secuencial directamente de la base de datos.
        /// </summary>
        /// <returns>El string del número de transacción generado (ejemplo: 01202510240001).</returns>
        /// <exception cref="InvalidOperationException">Lanzada si el contexto de la base de datos no ha sido inicializado.</exception>
        public string GenerateTransactionNumber()
        {
            if (_context == null)
            {
                throw new InvalidOperationException("ServiceHelper no ha sido inicializado. Debe llamar a Initialize() primero.");
            }

            // 1. Obtener el número secuencial de la base de datos de manera ATÓMICA
            int sequenceNumber = GetNextDailySequenceNumberFromDb();

            // 2. Obtener la fecha actual y darle formato
            DateTime now = DateTime.Now;
            string datePart = now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            // 3. Formato del número secuencial (4 dígitos con ceros a la izquierda)
            string sequencePart = sequenceNumber.ToString("D4");

            // 4. Concatenar todas las partes
            // Resultado: 01 + 20251024 + 0001
            return $"{TransactionIdPrefix}{datePart}{sequencePart}";
        }

        /// <summary>
        /// **MÉTODO CRÍTICO:** Llama al SP de SQL Server para obtener e incrementar
        /// el número de secuencia diario. ESTA LÓGICA DEBE ESTAR EN SQL SERVER.
        /// </summary>
        /// <returns>El próximo número secuencial (1, 2, 3, etc.).</returns>
        private int GetNextDailySequenceNumberFromDb()
        {
            var spName = "sp_GetNextDailySequence"; // Nombre del SP que maneja la secuencia diaria

            using (var connection = _context.CreateConnection())
            {
                // El SP debe devolver un único valor entero (el número de secuencia)
                // Ver el SP de SQL proporcionado en la respuesta anterior para la lógica interna.
                int sequence = connection.ExecuteScalar<int>(
                    spName,
                    commandType: CommandType.StoredProcedure
                );
                return sequence;
            }
        }
    }

    // Interfaz ficticia que usará tu contexto de Dapper.
    // Asumo que tienes una clase que implementa esto o un método similar.
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
