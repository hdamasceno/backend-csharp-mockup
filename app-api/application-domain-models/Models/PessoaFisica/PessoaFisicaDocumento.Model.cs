using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.PessoaFisica
{
    [Table("PessoaFisicaDocumento")]
    public class PessoaFisicaDocumentoModel : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid PessoaFisicaId { get; set; }
        public Guid DocumentoTipoId { get; set; }
        public string DocumentoNumero { get; set; } = string.Empty;
        public DateTime? DocumentoEmissaoData { get; set; }
        public DateTime? DocumentoValidadeData { get; set; }
        public Guid? DocumentoEmissaoOrgaoExpedidorId { get; set; }
        public Guid? DocumentoEmissaoEstadoId { get; set; }

        public PessoaFisicaDocumentoModel() : base() { }

        public PessoaFisicaDocumentoModel(string json)
        {
            LoadFromJSON<PessoaFisicaDocumentoModel>(this, json);
        }
    }
}