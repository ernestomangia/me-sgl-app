CREATE TABLE [dbo].[Cobro] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [FechaCobro]   DATETIME        NOT NULL,
    [Monto]        DECIMAL (18, 2) NOT NULL,
    [Estado]       INT             NOT NULL,
    [FechaAlta]    DATETIME        NOT NULL,
    [Rendicion_Id] BIGINT          NULL,
    CONSTRAINT [PK_dbo.Cobro] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Cobro_dbo.Rendicion_Rendicion_Id] FOREIGN KEY ([Rendicion_Id]) REFERENCES [dbo].[Rendicion] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Rendicion_Id]
    ON [dbo].[Cobro]([Rendicion_Id] ASC);

