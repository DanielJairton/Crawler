using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WebScrapping.Models;

public class MagazineLuizaScraper
{
    public List<string> ObterPreco(string descricaoProduto, int idProduto)
    {
        try
        {
            // Inicializa o ChromeDriver
            using (IWebDriver driver = new ChromeDriver())
            {
                // Abre a página
                driver.Navigate().GoToUrl($"https://www.magazineluiza.com.br/busca/{descricaoProduto}");

                // Aguarda um tempo fixo para permitir que a página seja carregada (você pode ajustar conforme necessário)
                System.Threading.Thread.Sleep(5000);

                // Encontra o elemento que possui o atributo data-testid
                IWebElement priceElement = driver.FindElement(By.CssSelector("[data-testid='price-value']"));

                // Encontra o elemento a com o atributo product-card-container
                IWebElement linkElement = driver.FindElement(By.CssSelector("a[data-testid='product-card-container']"));

                // Encontrar o elemento h2 com o atributo product-title
                IWebElement titleElement = driver.FindElement(By.CssSelector("h2[data-testid='product-title']"));

                // Verifica se o elemento foi encontrado
                if (priceElement != null && linkElement != null)
                {
                    // Obtém o preço do primeiro produto
                    string firstProductPrice = priceElement.Text;

                    //Obtém link do produto
                    string firstProductLink = linkElement.GetAttribute("href");

                    //Obtém titulo do produto
                    string firstProductName = titleElement.Text;
                    //Console.Write($"\n\nNome Magazine Luiza: {firstProductName}\n\n");

                    // Registra o log com o ID do produto
                    RegistrarLog("rDj1", "DanielJ", DateTime.Now, "WebScraping - Magazine Luiza", "Sucesso", idProduto);

                    //Teste mostrar preço do produto
                    //Console.WriteLine(firstProductPrice);

                    Console.WriteLine("Preço Magazine Luiza Obtido");

                    //Lista para retornar
                    List<string> lista = new List<string>
                    {
                        $"{firstProductPrice}",
                        $"{firstProductLink}",
                        $"{firstProductName}"
                    };

                    // Retorna o preço
                    //return firstProductPrice;

                    //Retorna lista com o preço e link
                    return lista;

                }
                else
                {
                    Console.WriteLine("Preço não encontrado.");

                    // Registra o log com o ID do produto
                    RegistrarLog("rDj1", "DanielJ", DateTime.Now, "WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);

                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

            // Registra o log com o ID do produto
            RegistrarLog("rDj1", "DanielJ", DateTime.Now, "Web Scraping - Magazine Luiza", $"Erro: {ex.Message}", idProduto);

            return null;
        }
    }

    private static void RegistrarLog(string codRob, string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
    {

        using (var context = new LogContext())
        {
            var log = new Log
            {
                CodRob = codRob,
                UsuRob = usuRob,
                DateLog = dateLog,
                Processo = processo,
                InfLog = infLog,
                IdProd = idProd
            };
            context.Logs.Add(log);
            context.SaveChanges();
        }

    }
}