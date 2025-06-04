using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient
{
    public static class SerilogConfig
    {
        public static ILogger Configure()
        {
            try
            {
                // Попробуем основной путь в папке проекта
                //string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "admin.log");
                string logPath = Path.Combine("C:\\Temp", "logs", "admin.log");
                string logDirectory = Path.GetDirectoryName(logPath);

                // Проверяем и создаём папку logs
                try
                {
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                        Console.WriteLine($"Создана папка для логов: {logDirectory}");
                    }
                }
                catch (Exception ex)
                {
                    // Если не удалось создать папку в директории проекта, пробуем альтернативный путь
                    Console.WriteLine($"Не удалось создать папку {logDirectory}: {ex.Message}");
                    logPath = logPath = Path.Combine("C:\\Temp", "logs", "admin.log");
                    logDirectory = Path.GetDirectoryName(logPath);
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                        Console.WriteLine($"Создана альтернативная папка для логов: {logDirectory}");
                    }
                }

                var logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console() // Вывод в консоль для отладки
                    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day) // Логи в файл
                    .CreateLogger();

                logger.Information("Serilog успешно настроен. Логи будут записываться в {LogPath}", logPath);
                return logger;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка настройки Serilog: {ex.Message}");
                // Создаём минимальный логгер, который пишет только в консоль
                var fallbackLogger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .CreateLogger();
                fallbackLogger.Error(ex, "Не удалось настроить Serilog с файлом логов");
                return fallbackLogger;
            }
        }
    }
}
