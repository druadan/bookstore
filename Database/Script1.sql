
--select DISTINCT * from Bookstore.dbo.Book b join Bookstore.dbo.Tag_association ta on ta.book_id = b.id and  title = 'Szerlok';
SELECT DISTINCT b.* FROM bookstore.dbo.Book b JOIN bookstore.dbo.Tag_association ta on ta.book_id = b.id  WHERE  title = @titleCond      tag_id = @tagCond ;
