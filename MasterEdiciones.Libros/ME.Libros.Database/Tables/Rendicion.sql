CREATE TABLE [dbo].[Rendicion] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Periodo]        DATETIME        NOT NULL,
    [MontoFacturado] DECIMAL (18, 2) NOT NULL,
    [MontoNeto]      DECIMAL (18, 2) NOT NULL,
    [Comision]       DECIMAL (18, 2) NOT NULL,
    [MontoComision]  DECIMAL (18, 2) NOT NULL,
    [FechaAlta]      DATETIME        NOT NULL,
    [Cobrador_Id]    BIGINT          NOT NULL,
    [Localidad_Id]   BIGINT          NOT NULL,
    CONSTRAINT [PK_dbo.Rendicion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Rendicion_dbo.Cobrador_Cobrador_Id] FOREIGN KEY ([Cobrador_Id]) REFERENCES [dbo].[Cobrador] ([Id]),
    CONSTRAINT [FK_dbo.Rendicion_dbo.Localidad_Localidad_Id] FOREIGN KEY ([Localidad_Id]) REFERENCES [dbo].[Localidad] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Localidad_Id]
    ON [dbo].[Rendicion]([Localidad_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Cobrador_Id]
    ON [dbo].[Rendicion]([Cobrador_Id] ASC);

