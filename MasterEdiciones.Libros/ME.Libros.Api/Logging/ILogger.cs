namespace ME.Libros.Api.Logging
{
    public interface ILogger
    {
        void Log(string mensaje, SeveridadLog severidad);
    }
}
