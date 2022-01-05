namespace application_domain.Interfaces
{
    public interface IResposta
    {
        public string Status { get; }
        public string? Message { get; }
        public object? Objeto { get; }

        public void ComandoExecutadoComSucesso();
        public void ComandoExecutadoComSucesso(string status);
        public void ComandoExecutadoComSucesso(string status, object objeto);
        public void ComandoExecutadoComSucesso(object objeto);
        public IResposta ComandoExecutadoComErro();
        public IResposta ComandoExecutadoComErro(string status, string message);
        public IResposta ComandoExecutadoComErro(string status, string message, object objeto);
        public IResposta ComandoExecutadoComErro(object objeto);

        public dynamic GetObjetoInDynamic();
        public string GetObjetoInJSON();
    }
}