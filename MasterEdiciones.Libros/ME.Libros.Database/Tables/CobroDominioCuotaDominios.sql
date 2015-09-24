CREATE TABLE [dbo].[CobroDominioCuotaDominios] (
    [CobroDominio_Id] BIGINT NOT NULL,
    [CuotaDominio_Id] BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.CobroDominioCuotaDominios] PRIMARY KEY CLUSTERED ([CobroDominio_Id] ASC, [CuotaDominio_Id] ASC),
    CONSTRAINT [FK_dbo.CobroDominioCuotaDominios_dbo.Cobro_CobroDominio_Id] FOREIGN KEY ([CobroDominio_Id]) REFERENCES [dbo].[Cobro] ([Id]),
    CONSTRAINT [FK_dbo.CobroDominioCuotaDominios_dbo.Cuota_CuotaDominio_Id] FOREIGN KEY ([CuotaDominio_Id]) REFERENCES [dbo].[Cuota] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CuotaDominio_Id]
    ON [dbo].[CobroDominioCuotaDominios]([CuotaDominio_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CobroDominio_Id]
    ON [dbo].[CobroDominioCuotaDominios]([CobroDominio_Id] ASC);

