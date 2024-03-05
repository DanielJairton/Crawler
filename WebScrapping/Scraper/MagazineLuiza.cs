using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using WebScrapping.Data;
using WebScrapping.Models;

public class MagazineLuizaScraper
{
    public List<string> ObterPreco(string descricaoProduto, int idProduto)
    {
        try
        {
            // Define as opções do ChromeDriver
            ChromeOptions options = new ChromeOptions();

            // Adiciona a flag --enable-chrome-browser-cloud-management
            options.AddArgument("--enable-chrome-browser-cloud-management");

            // Desabilite
            options.AddArgument("--disable-web-security");
            options.AddArgument("--ignore-certificate-errors");

            // Inicializa o ChromeDriver
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120); // Aumento do tempo limite para 120 segundos

                // Abre a página
                driver.Navigate().GoToUrl($"https://www.magazineluiza.com.br/busca/{descricaoProduto}");

                // Aguarda um tempo fixo para permitir que a página seja carregada (você pode ajustar conforme necessário)
                //15 segundos para o senai, 45 para minha casa, 20 segundos para garantir que dá tempo de rodar no computador de Reginaldo sem demorar demais
                //System.Threading.Thread.Sleep(5000);
                //System.Threading.Thread.Sleep(45000);
                System.Threading.Thread.Sleep(20000);

                // Encontra o elemento que possui o atributo data-testid
                IWebElement priceElement = driver.FindElement(By.CssSelector("[data-testid='price-value']"));

                // Encontra o elemento a com o atributo product-card-container
                IWebElement linkElement = driver.FindElement(By.CssSelector("a[data-testid='product-card-container']"));

                // Encontrar o elemento h2 com o atributo product-title
                IWebElement titleElement = driver.FindElement(By.CssSelector("h2[data-testid='product-title']"));

                // Verifica se o elemento foi encontrado
                if (priceElement != null && linkElement != null && titleElement != null)
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

                    //Retorna lista com o preço, link e nome
                    return lista;

                }
                else
                {
                    Console.WriteLine("Preço Magazine Luiza não encontrado.");

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

    private static void RegistrarLog(string codigoRobo, string usuarioRobo, DateTime dateLog, string etapa, string informacaoLog, int idProdutoAPI)
    {

        using (var context = new LogContext())
        {
            var log = new LOGROBO
            {
                CodigoRobo = codigoRobo,
                UsuarioRobo = usuarioRobo,
                DateLog = dateLog,
                Etapa = etapa,
                InformacaoLog = informacaoLog,
                IdProdutoAPI = idProdutoAPI
            };
            context.LOGROBO.Add(log);
            context.SaveChanges();
        }

    }
}