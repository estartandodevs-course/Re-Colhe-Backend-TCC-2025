namespace ReColhe.Application.Pev.Criar
{
    public class CriarPevCommandResponse
    {
        public int PevId { get; }

        public CriarPevCommandResponse(int pevId)
        {
            PevId = pevId;
        }
    }
}