using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Interfaces;
using application_domain.Types.Values;

namespace application_data_entities
{
    public class PessoaJuridica : Pessoa, IEntidadeBase
    {
        public Key? RegimeTributarioId { get; private set; }
        public virtual IEntidade? RegimeTributario { get; private set; }
        public Cnpj DocumentoCnpj { get; private set; }
        public InscricaoMunicipal? DocumentoInscricaoMunicipal { get; private set; }
        public Cnae? CNAE { get; private set; }
        public Name RazaoSocial { get; private set; }
        public Name? NomeFantasia { get; private set; }
        public Data? AberturaData { get; private set; }
        public virtual List<IEntidadeBase> DocumentoInscricaoEstadualList { get; set; } = new List<IEntidadeBase>();
        public virtual List<IEntidadeBase> EnderecoList { get; set; } = new List<IEntidadeBase>();
        public virtual List<IEntidadeBase> FilialList { get; set; } = new List<IEntidadeBase>();

        public PessoaJuridica(dynamic objetoDynamic)
        {
            Load(objetoDynamic);
        }

        public override void Load(dynamic objetoDynamic)
        {
            if (objetoDynamic == null)
            {
                AddNotification($"{GetType().Name}.Load", $"{GetType().Name} - JSON inválido.");

                return;
            }

            LoadFromDynamic<PessoaJuridica>(this, objetoDynamic);            

            if (IsValid)
            {
                AddNotifications(AccountId.contract, CadastradoDataHora.contract, DocumentoCnpj.contract, RazaoSocial.contract);

                if (AlteradoDataHora.HasValue)
                    AddNotifications(AlteradoDataHora?.contract);

                if (RegimeTributarioId.HasValue)
                    AddNotifications(RegimeTributarioId?.contract);

                if (DocumentoInscricaoMunicipal.HasValue)
                    AddNotifications(DocumentoInscricaoMunicipal?.contract);

                if (CNAE.HasValue)
                    AddNotifications(CNAE?.contract);

                if (NomeFantasia.HasValue)
                    AddNotifications(NomeFantasia?.contract);

                if (AberturaData.HasValue)
                    AddNotifications(AberturaData?.contract);

                if (FuncoesEspeciais.IsFieldExist(objetoDynamic, "DocumentoInscricaoEstadualList"))
                {
                    if (objetoDynamic != null)
                    {
                        foreach (var item in objetoDynamic.DocumentoInscricaoEstadualList)
                        {
                            var documentoInscricaoEstadual = new PessoaJuridicaInscricaoEstadual(item);

                            if (documentoInscricaoEstadual.IsValid)
                                DocumentoInscricaoEstadualList.Add(documentoInscricaoEstadual);
                        }
                    }
                }
            }
        }
    }
}
