CREATE DATABASE db_logRobo

USE db_logRobo

--Em nenhuma hipótese a estrutura do banco de dados deve ALTERADA. Também,
--é proibido criar novas tabelas no banco de dados

--Tabela de logs do robo
CREATE TABLE LOGROBO(
iDloG INT IDENTITY(1,1) PRIMARY KEY,
CodigoRobo Varchar(4) not null,
UsuarioRobo varchar(100) not null,
DateLog datetime not null,
Etapa varchar(100) not null,
InformacaoLog varchar(100) not null,
idProdutoAPI int
)