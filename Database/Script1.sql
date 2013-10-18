SELECT DISTINCT *
FROM bookstore.dbo.Book b 
JOIN Bookstore.dbo.Review r ON r.book_id = b.id 
JOIN Bookstore.dbo.Client c on r.customer_login = c.[login]



SELECT DISTINCT b.id, AVG(r.score)
FROM bookstore.dbo.Book b 
JOIN Bookstore.dbo.Review r ON r.book_id = b.id 
JOIN Bookstore.dbo.Client c on r.customer_login = c.[login]
WHERE  c.education = 'Podstawowe'
GROUP by b.id
HAVING AVG(r.score) >= 2