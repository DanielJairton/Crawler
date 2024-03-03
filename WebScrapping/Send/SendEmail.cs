using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Program;

namespace WebScrapping.Send
{
    public class SendEmail
    {
        public static void EnviarEmail(string nomeProdutoMercadoLivre, string precoProdutoMercadoLivre, string linkProdutoMercadoLivre, string nomeProdutoMagazineLuiza, 
            string precoProdutoMagazineLuiza, string linkProdutoMagazineLuiza, string nomeProduto, string melhorCompra, string destinaraio)
        {
            // Configurações do servidor SMTP do Email Outlook
            string smtpServer = "smtp.office365.com"; // Servidor SMTP do Outlook
            int porta = 587; // Porta SMTP do Outlook para TLS/STARTTLS
            //O email outlook tem um limite de emails diários que quando ultrapassado precisa receber um código enviado ao celular para continuar
            string remetente = "danielTesteSenai@outlook.com"; // Seu endereço de e-mail Outlook
            string senha = "caderno6060!"; // Sua senha do email Outlook

            // Configurar cliente SMTP
            using (SmtpClient client = new SmtpClient(smtpServer, porta))
            {
                client.UseDefaultCredentials = false;
                client.Port = porta;
                client.Credentials = new NetworkCredential(remetente, senha);
                client.EnableSsl = true; // Habilitar SSL/TLS

                // Construir mensagem de e-mail
                MailMessage mensagem = new MailMessage(remetente, destinaraio)
                {
                    Subject = "Resultado da comparação de preços",
                    Body = $"Produto Pesquisado: {nomeProduto}\n\n" +
                           $"Produto do Mercado Livre: {nomeProdutoMercadoLivre} - Preço: {precoProdutoMercadoLivre}\n" +
                           $"Link Mercado Livre: {linkProdutoMercadoLivre}\n\n" +
                           $"Produto do Magazine Luiza: {nomeProdutoMagazineLuiza} - Preço: {precoProdutoMagazineLuiza}\n" +
                           $"Link Magazine Luiza: {linkProdutoMagazineLuiza}\n\n" +
                           $"Melhor Compra: {melhorCompra}\n\n" +
                           "Robô rDj1\n" +
                           "Usuário DanielJ\n"

                };

                // Enviar e-mail
                client.Send(mensagem);

            }
        }

    }
}