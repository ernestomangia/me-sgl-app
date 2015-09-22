CREATE TABLE [dbo].[Usuario] (
    [Id]                       BIGINT        IDENTITY (1, 1) NOT NULL,
    [Nombre]                   NVARCHAR (80) NOT NULL,
    [Apellido]                 NVARCHAR (80) NOT NULL,
    [UserName]                 NVARCHAR (50) NOT NULL,
    [Password]                 NVARCHAR (50) NOT NULL,
    [Email]                    NVARCHAR (80) NULL,
    [EmailConfirmado]          BIT           NOT NULL,
    [Habilitado]               BIT           NOT NULL,
    [UltimoLogin]              DATETIME      NULL,
    [CantidadIntentosFallidos] BIGINT        NOT NULL,
    [FechaAlta]                DATETIME      NOT NULL,
    CONSTRAINT [PK_dbo.Usuario] PRIMARY KEY CLUSTERED ([Id] ASC)
);


