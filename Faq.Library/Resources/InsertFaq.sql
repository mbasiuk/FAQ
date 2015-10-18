INSERT INTO FAQ(Answer, Question)
VALUES(@answer, @question)
SELECT SCOPE_IDENTITY()