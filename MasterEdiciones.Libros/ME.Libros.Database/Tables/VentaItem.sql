CREATE TABLE [dbo].[VentaItem] (
    [Id]                   BIGINT          IDENTITY (1, 1) NOT NULL,
    [Orden]                INT             NOT NULL,
    [Cantidad]             INT             NOT NULL,
    [PrecioCosto]          DECIMAL (18, 2) NOT NULL,
    [PrecioVentaCalculado] DECIMAL (18, 2) NOT NULL,
    [PrecioVentaVendido]   DECIMAL (18, 2) NOT NULL,
    [MontoCalculado]       DECIMAL (18, 2) NOT NULL,
    [MontoVendido]         DECIMAL (18, 2) NOT NULL,
    [FechaAlta]            DATETIME        NOT NULL,
    [Producto_Id]          BIGINT          NOT NULL,
    [Venta_Id]             BIGINT          NOT NULL,
    CONSTRAINT [PK_dbo.VentaItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.VentaItem_dbo.Producto_Producto_Id] FOREIGN KEY ([Producto_Id]) REFERENCES [dbo].[Producto] ([Id]),
    CONSTRAINT [FK_dbo.VentaItem_dbo.Venta_Venta_Id] FOREIGN KEY ([Venta_Id]) REFERENCES [dbo].[Venta] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Venta_Id]
    ON [dbo].[VentaItem]([Venta_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Producto_Id]
    ON [dbo].[VentaItem]([Producto_Id] ASC);

