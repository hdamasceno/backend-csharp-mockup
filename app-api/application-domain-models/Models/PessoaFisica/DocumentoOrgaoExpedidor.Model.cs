using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca;
using application_data_models.Models._Base;

namespace application_data_models.Models.PessoaFisica
{
    [Table("DocumentoOrgaoExpedidor")]
    public class DocumentoOrgaoExpedidorModel : BaseModel
    {
        public string Nome { get; set; } = string.Empty;
        public decimal AppCodigo { get; set; }
        public decimal SngpcCodigo { get; set; }
        public decimal NFeCodigo { get; set; }
        public decimal NFCeCodigo { get; set; }
        public decimal SpedFiscalCodigo { get; set; }
        public decimal SpedContribuicaoCodigo { get; set; }
        public decimal SintegraCodigo { get; set; }
        public decimal SatCodigo { get; set; }

        public DocumentoOrgaoExpedidorModel() : base() { }

        public DocumentoOrgaoExpedidorModel(string json)
        {
            LoadFromJSON<DocumentoOrgaoExpedidorModel>(this, json);
        }
    }
}