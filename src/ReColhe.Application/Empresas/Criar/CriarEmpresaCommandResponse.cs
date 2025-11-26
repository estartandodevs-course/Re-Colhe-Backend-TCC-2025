namespace ReColhe.Application.Empresas.Criar
{
    public class CriarEmpresaCommandResponse
    {
        public int EmpresaId { get; private set; }

        public CriarEmpresaCommandResponse(int empresaId)
        {
            EmpresaId = empresaId;
        }
    }
}