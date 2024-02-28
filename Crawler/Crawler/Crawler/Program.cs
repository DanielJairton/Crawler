using HtmlAgilityPack;

class Progam
{
    static async Task Main()
    {
        //URL dos sites
        string mercadolivreSite = "https://lista.mercadolivre.com.br/impressora#D[A:impressora]";
        string magazineLuizaSite = "https://www.magazineluiza.com.br/busca/impressora/";

        //Variáveis para armazenar informações do produto mercado livre
        string nomeProdutoMercadoLivre = "";
        string precoProdutoMercadoLivre = "";

        //Criar cliente HTTP para o mercado livre
        using (HttpClient client = new HttpClient ()) 
        {
        //Fazendo requisiçãopara o mercado livre
        HttpResponseMessage response = await client.GetAsync(mercadolivreSite);
            if (response.IsSuccessStatusCode) 
            {
                //Ler conteúdo HTML
                string html = await response.Content.ReadAsStringAsync();

                //Analisar o HTML
                HtmlDocument document = new HtmlDocument ();
                document.LoadHtml (html);

                //Pesquisar nome do produto
                //Pego com copiar xpath no elemento ao inspecionar
                HtmlNode nomeNode = document.DocumentNode.SelectSingleNode("//*[@id=\":Rh59bb:\"]/div[2]/div[1]/div[1]/a/h2");
                //HtmlNode nomeNode = document.DocumentNode.SelectNodes("//*[@id=\":Rh59bb:\"]/div[2]/div[1]/div[1]/a/h2").First();
                nomeProdutoMercadoLivre = nomeNode.InnerText.Trim ();

                //Pesquisar preço do produto
                //Pego com copiar xpath no elemento ao inspecionar
                HtmlNode precoNode = document.DocumentNode.SelectSingleNode("//*[@id=\":Rh59bb:\"]/div[2]/div[1]/div[2]/div/div/div/span[1]/span[2]");
                //HtmlNode precoNode = document.DocumentNode.SelectNodes("//*[@id=\":Rh59bb:\"]/div[2]/div[1]/div[2]/div/div/div/span[1]/span[2]").First();
                precoProdutoMercadoLivre = precoNode.InnerText.Trim();
            }
            else
            {
                Console.WriteLine("Falha ao fazer requisição Mercado Livre");
            }

            Console.WriteLine("Mercado livre");
            Console.WriteLine(nomeProdutoMercadoLivre);
            Console.WriteLine(precoProdutoMercadoLivre);
            Console.WriteLine();

            
            //Variáveis para armazenar informações do produto magazine luiza
            string nomeProdutoMagazineLuiza = "";
            string precoProdutoMagazineLuiza = "";

            //Criar cliente HTTP para o mercado livre
            using (HttpClient clientMagazine = new HttpClient ()) 
            {
            //Fazendo requisiçãopara o mercado livre
            HttpResponseMessage responseMagazine = await clientMagazine.GetAsync(magazineLuizaSite);
                if (responseMagazine.IsSuccessStatusCode) 
                {
                    //Ler conteúdo HTML da Magazine Luiza
                    string htmlMagazine = await responseMagazine.Content.ReadAsStringAsync ();

                    //Analisar html Magazine Luiza
                    HtmlDocument documentMagazine = new HtmlDocument ();
                    documentMagazine.LoadHtml (htmlMagazine);

                    //Pesquisar nome do produto
                    HtmlNode nomeMagazineNode = documentMagazine.DocumentNode.SelectSingleNode("//*[@id='__next']/div/main/section[4]/div[4]/div/ul/li[1]/a/div[3]/h2");
                    nomeProdutoMagazineLuiza = nomeMagazineNode.InnerText.Trim();

                    //Pesquisar preço do produto
                    HtmlNode precoMagazineNode = documentMagazine.DocumentNode.SelectSingleNode("//*[@id='__next']/div/main/section[4]/div[4]/div/ul/li[1]/a/div[3]/div[2]/div/p[1]");
                    precoProdutoMagazineLuiza = precoMagazineNode.InnerText.Trim();
                }
                else
                {
                    Console.WriteLine("Falha ao fazer requisição Magazine Luiza");
                }
                Console.WriteLine("Magazine Luiza");
                Console.WriteLine(nomeProdutoMagazineLuiza);
                Console.WriteLine(precoProdutoMagazineLuiza);

                //Console.WriteLine();
            }
        }
    }
}