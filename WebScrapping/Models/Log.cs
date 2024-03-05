using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapping.Models
{
    // Classe de modelo para os logs
    public class LOGROBO
    {
        [Key]
        public int iDlOG { get; set; }
        public string CodigoRobo { get; set; }
        public string UsuarioRobo { get; set; }
        public DateTime DateLog { get; set; }
        public string Etapa { get; set; }
        public string InformacaoLog { get; set; }
        public int IdProdutoAPI { get; set; }
    }
}
