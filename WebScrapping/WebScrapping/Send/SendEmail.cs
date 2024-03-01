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
        public static void EnviarEmail(string nomeProdutoMercadoLivre, string precoProdutoMercadoLivre, string linkProdutoMercadoLivre, string nomeProdutoMagazineLuiza, string precoProdutoMagazineLuiza, string nomeProduto, string melhorCompra)
        {
            // Configurações do servidor SMTP do Gmail
            string smtpServer = "smtp.office365.com"; // Servidor SMTP do Gmail
            int porta = 587; // Porta SMTP do Gmail para TLS/STARTTLS
            string remetente = "danielTesteSenai@outlook.com"; // Seu endereço de e-mail do Gmail
            string senha = "caderno6060!"; // Sua senha do Gmail

            // Configurar cliente SMTP
            using (SmtpClient client = new SmtpClient(smtpServer, porta))
            {
                client.UseDefaultCredentials = false;
                client.Port = porta;
                client.Credentials = new NetworkCredential(remetente, senha);
                client.EnableSsl = true; // Habilitar SSL/TLS

                // Construir mensagem de e-mail
                MailMessage mensagem = new MailMessage(remetente, "danielTesteSenai@outlook.com")
                {
                    Subject = "Resultado da comparação de preços",
                    Body = $"Produto Pesquisado: {nomeProduto}\n" +
                           $"Produto do Mercado Livre: {nomeProdutoMercadoLivre} - Preço: {precoProdutoMercadoLivre}\n" +
                           $"Link: " +
                           $"Produto do Magazine Luiza: {nomeProdutoMagazineLuiza} - Preço: {precoProdutoMagazineLuiza}\n\n" +
                           $"Melhor Compra: {melhorCompra}\n" +
                           "Robô rDj1\n" +
                           "Usuário DanielJ\n"

                };

                // Enviar e-mail
                client.Send(mensagem);


                Console.WriteLine(nomeProdutoMercadoLivre);
                Console.WriteLine(precoProdutoMercadoLivre);
                Console.WriteLine(nomeProdutoMagazineLuiza);
                Console.WriteLine(precoProdutoMagazineLuiza);

            }
        }
        //EnviarEmail(nomeProdutoMercadoLivre, precoProdutoMercadoLivre, nomeProdutoMagazineLuiza, precoProdutoMagazineLuiza);

    }
}

// Enviar email com o resultado da comparação
//    EnviarEmail(nomeProdutoMercadoLivre, precoProdutoMercadoLivre, nomeProdutoMagazineLuiza, precoProdutoMagazineLuiza);
//}

//static void EnviarEmail(string nomeProdutoMercadoLivre, string precoProdutoMercadoLivre, string nomeProdutoMagazineLuiza, string precoProdutoMagazineLuiza)
//{
//    // Configurações do servidor SMTP do Gmail
//    string smtpServer = "smtp-mail.outlook.com"; // Servidor SMTP do Gmail
//    int porta = 587; // Porta SMTP do Gmail para TLS/STARTTLS
//    string remetente = "wallacemaximus@hotmail.com"; // Seu endereço de e-mail do Gmail
//    string senha = "teste"; // Sua senha do Gmail

//    // Configurar cliente SMTP
//    using (SmtpClient client = new SmtpClient(smtpServer, porta))
//    {
//        client.UseDefaultCredentials = false;
//        client.Credentials = new NetworkCredential(remetente, senha);
//        client.EnableSsl = true; // Habilitar SSL/TLS

//        // Construir mensagem de e-mail
//        MailMessage mensagem = new MailMessage(remetente, "wallace@docente.senai.br")
//        {
//            Subject = "Resultado da comparação de preços",
//            Body = $"Produto do Mercado Livre: {nomeProdutoMercadoLivre} - Preço: {precoProdutoMercadoLivre}\n" +
//                   $"Produto do Magazine Luiza: {nomeProdutoMagazineLuiza} - Preço: {precoProdutoMagazineLuiza}\n"

//        };

//        // Enviar e-mail
//        client.Send(mensagem);


/*Console.WriteLine(nomeProdutoMercadoLivre);
Console.WriteLine(precoProdutoMercadoLivre);
Console.WriteLine(nomeProdutoMagazineLuiza);
Console.WriteLine(precoProdutoMagazineLuiza);*/

//  }