﻿CREATE TABLE [dbo].[Login]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Usuario] NVARCHAR(20) NOT NULL, 
    [Contrasena] NVARCHAR(20) NOT NULL
)
