-- fixes ENERGYWIZ-71
ALTER table Response
Drop constraint DF_Response_Description

ALTER table Response
alter column Description nvarchar(max) NOT NULL

alter table Response
add CONSTRAINT [DF_Response_Description] DEFAULT ('') for Description

