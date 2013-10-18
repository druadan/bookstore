DROP database Bookstore;
CREATE database Bookstore;
USE Bookstore;

DROP TABLE [Order_detail];
DROP TABLE [Order];
DROP TABLE [Review];
DROP TABLE [Tag_association];
DROP TABLE [Client];
DROP TABLE [Book];
DROP TABLE [Salesman];
DROP TABLE [Tag];
DROP TABLE [Category];
DROP TABLE [Education];

CREATE TABLE [Category]
(
	[name] varchar(30) NOT NULL PRIMARY KEY
)

INSERT into Category
VALUES 
--( 'Piotr','Reszke','Luzino', 0, HASHBYTES('SHA1','pr')),
--( 'Olga','M','Luzino', 1, HASHBYTES('SHA1','om'))
( 'Kucharska'),
( 'Romans'),
( 'Horror'),
( 'Melodramat'),
( 'Biografia'),
( 'Science-fiction'),
( 'Fantasy'),
( 'Dramat'),
( 'Przygodowa' )
;

CREATE TABLE [Education]
(
	[name] varchar(30) NOT NULL PRIMARY KEY
)

INSERT into Education
VALUES 
( 'Podstawowe' ),
( 'Gimnazjalne'),
( 'Zasadnicze zawodowe'),
( 'Średnie' ),
( 'Wyższe')
;

CREATE TABLE [Client]
(
	[login] varchar(30) NOT NULL PRIMARY KEY, 
	name varchar(30), 
	surname varchar(50),
	[address] varchar(100),
	loyal_client int,
	[password] varbinary(8000) NOT NULL,
	age int not null,
	education varchar(30) NOT NULL,
	preferredCat varchar(30) not null,
	preferredCat2 varchar(30) not null,
	foreign key (preferredCat ) references Category (name),
	foreign key (preferredCat2 ) references Category(name),
	foreign key (education ) references Education(name),
)

INSERT into Client
VALUES 
--( 'Piotr','Reszke','Luzino', 0, HASHBYTES('SHA1','pr')),
--( 'Olga','M','Luzino', 1, HASHBYTES('SHA1','om'))
--( 'pr','Piotr','Reszke','Gdańsk', 0, 'pr', 34, 'Podstawowe','Dramat','Przygodowa'),
( 'om','Olga', 'Kowalska','Wrocław', 1, HASHBYTES('SHA1','om'), 12, 'Wyższe','Kucharska','Fantasy')
--( 'dk','Dawid', 'Kotłowski','Poznań', 0, 'dk',62, 'Średnie','Przygodowa','Biografia'),
--( 'ws','Wojtek', 'Wszelki','Warszawa', 0, 'ws',25, 'Średnie','Horror','Biografia'),
--( 'fm','Filip', 'Macek','Rzeszów', 0, 'fm',43, 'Podstawowe','Horror','Biografia')
;

CREATE TABLE [Book]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	title varchar(150), 
	author varchar(50),
	category varchar(30),
	price float,
	foreign key (category ) references Category (name),
)

INSERT into Book(title, author, category, price)
VALUES 
( 'Sherlock Holmes', 'Arthur Conan Doyle', 'Przygodowa',66),
( 'Inferno', 'Dan Brown', 'Przygodowa', 23),
( 'Władca Pierścieni', 'John Ronald Reuel Tolkien', 'Fantasy', 34),
( 'Ojciec Chrzestny', 'Mario Puzo', 'Dramat', 88),
( 'Duma i uprzedzenie', 'Jane Austen', 'Melodramat', 83),
( 'Kod Leonarda Da Vinci', 'Dan Brown', 'Przygodowa', 26),
( 'Alicja w Krainie Czarów', 'Lewis Carroll', 'Fantasy', 44),
( 'Hamlet', 'William Shakespeare', 'Dramat', 120),
( 'Wielki Gatsby', 'William Shakespeare', 'Melodramat', 92),
( 'Rok 1984', 'George Orwell', 'Science-fiction', 61)
;


CREATE TABLE [Salesman]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	name varchar(30), 
	surname varchar(30),
)

INSERT into Salesman(name, surname)
VALUES 
( 'Dawid', 'Bombello'),
( 'Wojtek', 'Grommenstein')
;


CREATE TABLE [Tag]
(
	tag_id varchar(30) NOT NULL PRIMARY KEY, 
)

INSERT into Tag(tag_id)
VALUES 
( 'omijać' ),
( 'polecam' ),
( 'tylkodladesperatów' ),
( 'najwyższapółka' ),
( 'ulubiona' ),
( 'żałujęzakupu' ),
( 'przeczytałemwjednąnoc' ),
( 'pasjonująca' ),
( 'klasykagatunku' )
;

CREATE TABLE [Order]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	customer_login varchar(30) NOT NULL, 
	order_date date, 
	salesman_id int,
	foreign key (customer_login ) references Client ([login]),
	foreign key (salesman_id ) references Salesman (id)
)


INSERT into [Order](customer_login, order_date, salesman_id)
VALUES 
( 'pr', '2013-12-01', 1 ),
( 'om', '2011-04-21', 1 )
;


CREATE TABLE [Order_detail]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	order_id int NOT NULL, 
	book_id int NOT NULL,
	quantity int NOT NULL,
	foreign key ( order_id ) references [Order] (id),
	foreign key ( book_id ) references Book (id)
)

INSERT into [Order_detail] (order_id, book_id, quantity)
VALUES 
( 1, 2, 1 ),
( 2, 1, 3 ),
( 2, 3, 1 )
;

CREATE TABLE [Review]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1), 
	customer_login varchar(30) NOT NULL, 
	book_id int NOT NULL,
	title varchar(50) NOT NULL,
	content varchar(8000) NOT NULL,
	score float NOT NULL,
	foreign key ( customer_login ) references Client ([login]),
	foreign key ( book_id ) references Book (id)
)

INSERT into [Review] (customer_login, book_id, title, content, score)
VALUES 
( 'pr', 2, 'no nie wiem', 'mieszanie odczucia, mogla by być fajniejsza', 3.0),
( 'om', 1, 'super', 'polecam każdemu', 4.5 ),
( 'om', 2, 'troche nudna', 'ale da się przeczytac', 3.0 ),
( 'dk', 4, 'czytałem z wypiekami na twarzy', 'jedna z lepszych książek jakie czytałem', 4.5 ),
( 'ws', 6, 'polecam', 'nie każdemu się spodoba, ale za tą cenę warto sprobówać', 3.5 ),
( 'fm', 8, 'dlaczego', 'jest tak mało, tak świetnych książek' ,5.0 ),
( 'dk', 8, 'porażka', 'porażka', 2.0 )
;

CREATE TABLE [Tag_association]
(
	id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	tag_id varchar(30) NOT NULL,
	book_id int NOT NULL,
	foreign key ( book_id ) references Book (id),
	foreign key ( tag_id ) references Tag (tag_id)
)

INSERT into [Tag_association] (tag_id, book_id)
VALUES 
( 'omijać', 2),
( 'żałujęzakupu', 1),
( 'pasjonująca', 1),
( 'najwyższapółka', 2),
( 'omijać', 9),
( 'polecam', 8),
( 'przeczytałemwjednąnoc', 7),
( 'klasykagatunku', 3),
( 'ulubiona', 3),
( 'najwyższapółka', 4),
( 'polecam', 8),
( 'pasjonująca', 10),
( 'tylkodladesperatów', 4),
( 'pasjonująca', 3),
( 'polecam', 8),
( 'przeczytałemwjednąnoc', 2)
;


DROP login adm ;
CREATE login adm with password = 'adm';
DROP USER adm;
CREATE USER adm from login adm;
EXEC sp_addrolemember 'db_owner', 'adm'
