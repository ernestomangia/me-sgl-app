CREATE TABLE [dbo].[Compra] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [FechaCompra]    DATETIME        NOT NULL,
    [Estado]         INT             NOT NULL,
    [MontoCalculado] DECIMAL (18, 2) NOT NULL,
    [MontoComprado]  DECIMAL (18, 2) NOT NULL,
    [NroRemito]      NVARCHAR (MAX)  NOT NULL,
    [NroFactura]     NVARCHAR (MAX)  NULL,
    [FechaAlta]      DATETIME        NOT NULL,
    [Proveedor_Id]   BIGINT          NOT NULL,
    CONSTRAINT [PK_dbo.Compra] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Compra_dbo.Proveedor_Proveedor_Id] FOREIGN KEY ([Proveedor_Id]) REFERENCES [dbo].[Proveedor] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Proveedor_Id]
    ON [dbo].[Compra]([Proveedor_Id] ASC);

