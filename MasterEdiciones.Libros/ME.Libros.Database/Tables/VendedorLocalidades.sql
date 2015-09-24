CREATE TABLE [dbo].[VendedorLocalidades] (
    [Vendedor_Id]  BIGINT NOT NULL,
    [Localidad_Id] BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.VendedorLocalidades] PRIMARY KEY CLUSTERED ([Vendedor_Id] ASC, [Localidad_Id] ASC),
    CONSTRAINT [FK_dbo.VendedorLocalidades_dbo.Localidad_Localidad_Id] FOREIGN KEY ([Localidad_Id]) REFERENCES [dbo].[Localidad] ([Id]),
    CONSTRAINT [FK_dbo.VendedorLocalidades_dbo.Vendedor_Vendedor_Id] FOREIGN KEY ([Vendedor_Id]) REFERENCES [dbo].[Vendedor] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Localidad_Id]
    ON [dbo].[VendedorLocalidades]([Localidad_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Vendedor_Id]
    ON [dbo].[VendedorLocalidades]([Vendedor_Id] ASC);

