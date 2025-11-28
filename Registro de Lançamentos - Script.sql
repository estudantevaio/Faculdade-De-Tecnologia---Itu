CREATE database ControleFinanceiro
GO
USE ControleFinanceiro
GO

CREATE login financas WITH password='senha123';
CREATE user financas FROM login financas;

EXEC sp_addrolemember 'DB_DATAREADER', 'financas';
EXEC sp_addrolemember 'DB_DATAWRITER', 'financas';

CREATE table Lancamentos (
id int not null identity (1,1) constraint pk_lancamentos primary key, 
descricao varchar (100) not null, 
valor decimal (9,2) not null, 
dataLancamento date, 
tipo varchar (7) not null constraint ck_lancamentos_tipo check (tipo in ('Entrada', 'Saída'))
)




