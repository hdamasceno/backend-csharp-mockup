using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Biblioteca 
{
	public static partial class FuncoesEspeciais
	{
		private static string SendGrid_API_Key = "<send-grid-api-key>";

        public static async Task<object> SendGrid_Email_Enviar_Interno(string emailOrigem, 
            string nomeOrigem, 
            string emailDestino, 
            string emailDestinoCoo, 
            string assunto, 
            string texto, 
            string textoHtml, 
            string objetoJson)
        {
            var msg = new SendGridMessage();

            if (string.IsNullOrWhiteSpace(emailOrigem))
                throw new Exception("Email de origem não enviado por parâmetro");

            if (string.IsNullOrWhiteSpace(nomeOrigem))
                throw new Exception("Nome de origem não enviado por parâmetro");

            msg.SetFrom(new EmailAddress(emailOrigem, FuncoesEspeciais.NomePessoaPrimeiraPalavra(nomeOrigem)));

            var recipients = new List<EmailAddress>();
            var recipientsCoo = new List<EmailAddress>();

            if (emailDestino.Contains(";"))
            {
                var listaDestino = emailDestino.Split(';');

                foreach (var item in listaDestino)
                {
                    if (string.IsNullOrWhiteSpace(item) == false)
                        recipients.Add(new EmailAddress(item, ""));
                }
            }
            else
            {
                recipients.Add(new EmailAddress(emailDestino, ""));
            }

            if (string.IsNullOrWhiteSpace(emailDestinoCoo) == false)
            {
                if (emailDestinoCoo.Contains(";"))
                {
                    var listaDestino = emailDestinoCoo.Split(';');

                    foreach (var item in listaDestino)
                    {
                        if (string.IsNullOrWhiteSpace(item) == false)
                            recipientsCoo.Add(new EmailAddress(item, ""));
                    }
                }
                else
                {
                    recipientsCoo.Add(new EmailAddress(emailDestinoCoo, ""));
                }
            }

            msg.AddTos(recipients);

            if (recipientsCoo.Count > 0)
                msg.AddBccs(recipientsCoo);

            msg.SetSubject(assunto);

            var objetoDynamic = FuncoesEspeciais.WebApi_Json_Deserializar<dynamic>(objetoJson);

            string anexoHtml = string.Empty;

            if (objetoDynamic != null)
            {
                foreach (var item in objetoDynamic.arquivosAnexados)
                {
                    string caminhoNomeArquivoAzure = FuncoesEspeciais.ToString(item.caminhoNomeArquivoAzure, false, false);
                    string arquivoNome = FuncoesEspeciais.ToString(item.arquivoNome);

                    if (System.IO.File.Exists(caminhoNomeArquivoAzure))
                    {
                        byte[] arquivoByte = FuncoesEspeciais.Arquivo_LerArquivoBinario(caminhoNomeArquivoAzure);
                        string sampleContent = Convert.ToBase64String(arquivoByte);

                        msg.AddAttachment(arquivoNome, sampleContent);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(anexoHtml) == false)
            {
                textoHtml += anexoHtml;
            }

            msg.AddContent(MimeType.Text, texto);
            msg.AddContent(MimeType.Html, textoHtml);

            var client = new SendGridClient(SendGrid_API_Key);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                var objeto = new
                {
                    sendGridEmailId = "teste",
                    headers = response.Headers
                };

                return objeto;
            }
            else
            {
                var objeto = new
                {
                    statusCode = response.StatusCode.ToString(),
                    emailDestino = emailDestino,
                    emailDestinoCoo = emailDestinoCoo
                };

                return objeto;
            }
        }
	}
}