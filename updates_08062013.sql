---
ALTER TABLE PreEvaluationQuestion drop CONSTRAINT DF_PreEvaluationQuestionID_Description
go

alter table PreEvaluationQuestion alter column Description nvarchar(max) not null
go

ALTER TABLE [dbo].[PreEvaluationQuestion] ADD  CONSTRAINT [DF_PreEvaluationQuestionID_Description]  DEFAULT ('') FOR [Description]
GO

---

alter table Question drop constraint DF_Question_Description
go

alter table Question alter column Description nvarchar(max) not null
GO

ALTER TABLE [dbo].[Question] ADD  CONSTRAINT [DF_Question_Description]  DEFAULT ('') FOR [Description]
GO
