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
                AddNotification($"{GetType().Name}.LoadFromDynamic", $"{GetType().Name} - JSON inválido.");

                return;
            }

            DataHora cadastradoDataHora = FuncoesEspeciais.ToDateTime(objetoDynamic?.CadastradoDataHora, false, false, true);
            DataHora? alteradoDataHora = FuncoesEspeciais.ToDateTimeNull(objetoDynamic?.AlteradoDataHora, false, false, true);
            Key accountId = FuncoesEspeciais.ToGuid(objetoDynamic?.AccountId);
            Key? regimeTributarioId = FuncoesEspeciais.ToGuidOrNull(objetoDynamic?.RegimeTributarioId);
            Cnpj documentoCnpj = FuncoesEspeciais.ToString(objetoDynamic?.DocumentoCnpj);
            InscricaoMunicipal? documentoInscricaoMunicipal = FuncoesEspeciais.ToString(objetoDynamic?.DocumentoInscricaoMunicipal);
            Cnae? cnae = FuncoesEspeciais.ToString(objetoDynamic?.CNAE);
            Name razaoSocial = FuncoesEspeciais.ToString(objetoDynamic?.RazaoSocial);
            Name? nomeFantasia = FuncoesEspeciais.ToString(objetoDynamic?.NomeFantasia);
            Data? aberturaData = FuncoesEspeciais.ToDateTime(objetoDynamic?.AberturaData, true, false, true);

            AddNotifications(accountId.contract, cadastradoDataHora.contract, documentoCnpj.contract, razaoSocial.contract);

            if (alteradoDataHora.HasValue)
                AddNotifications(alteradoDataHora?.contract);

            if (regimeTributarioId.HasValue)
                AddNotifications(regimeTributarioId?.contract);

            if (documentoInscricaoMunicipal.HasValue)
                AddNotifications(documentoInscricaoMunicipal?.contract);

            if (cnae.HasValue)
                AddNotifications(cnae?.contract);

            if (nomeFantasia.HasValue)
                AddNotifications(nomeFantasia?.contract);

            if (aberturaData.HasValue)
                AddNotifications(aberturaData?.contract);

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

            if (IsValid)
            {
                AccountId = accountId;
                RegimeTributarioId = regimeTributarioId;
                DocumentoCnpj = documentoCnpj;
                DocumentoInscricaoMunicipal = DocumentoInscricaoMunicipal;
                CNAE = cnae;
                RazaoSocial = razaoSocial;
                NomeFantasia = nomeFantasia;
                AberturaData = aberturaData;
            }
        }
    }
}
