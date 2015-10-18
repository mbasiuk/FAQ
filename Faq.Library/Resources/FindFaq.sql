--DECLARE @Pattern  NVARCHAR(50) = 'hello'
SELECT TOP 7 Id, Answer, Question
FROM Faq
WHERE Answer like @Pattern OR Question like @Pattern