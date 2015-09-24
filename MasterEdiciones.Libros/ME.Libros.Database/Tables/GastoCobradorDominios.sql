CREATE TABLE [dbo].[GastoCobradorDominios] (
    [Id]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [FechaGasto]          DATETIME        NOT NULL,
    [Monto]               DECIMAL (18, 2) NOT NULL,
    [FechaAlta]           DATETIME        NOT NULL,
    [Cobrador_Id]         BIGINT          NULL,
    [Gasto_Id]            BIGINT          NULL,
    [RendicionDominio_Id] BIGINT          NULL,
    CONSTRAINT [PK_dbo.GastoCobradorDominios] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.GastoCobradorDominios_dbo.Cobrador_Cobrador_Id] FOREIGN KEY ([Cobrador_Id]) REFERENCES [dbo].[Cobrador] ([Id]),
    CONSTRAINT [FK_dbo.GastoCobradorDominios_dbo.Gasto_Gasto_Id] FOREIGN KEY ([Gasto_Id]) REFERENCES [dbo].[Gasto] ([Id]),
    CONSTRAINT [FK_dbo.GastoCobradorDominios_dbo.Rendicion_RendicionDominio_Id] FOREIGN KEY ([RendicionDominio_Id]) REFERENCES [dbo].[Rendicion] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_RendicionDominio_Id]
    ON [dbo].[GastoCobradorDominios]([RendicionDominio_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Gasto_Id]
    ON [dbo].[GastoCobradorDominios]([Gasto_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Cobrador_Id]
    ON [dbo].[GastoCobradorDominios]([Cobrador_Id] ASC);

