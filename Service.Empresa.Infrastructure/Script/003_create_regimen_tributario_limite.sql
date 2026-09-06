-- ============================================================================
-- Service.Empresa - Tabla de Límites Tributarios Anuales por Régimen (SUNAT)
-- Motor: PostgreSQL
-- ============================================================================

CREATE TABLE IF NOT EXISTS empresa.regimen_tributario_limite (
    id_limite                   SERIAL NOT NULL,
    codigo_regimen_tributario   VARCHAR(10) NOT NULL,
    anio                        INT NOT NULL,
    valor_uit                   NUMERIC(10, 2) NOT NULL,
    limite_mensual_ventas       NUMERIC(12, 2),
    limite_mensual_compras      NUMERIC(12, 2),
    limite_anual_ventas         NUMERIC(12, 2),
    limite_anual_compras        NUMERIC(12, 2),
    limite_anual_ventas_uit     INT,
    activo                      BOOLEAN NOT NULL DEFAULT TRUE,
    creado_por                  VARCHAR(150),
    fecha_creacion              TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modificado_por              VARCHAR(150),
    fecha_modificacion          TIMESTAMP,

    CONSTRAINT pk_regimen_tributario_limite PRIMARY KEY (id_limite),
    CONSTRAINT fk_limite_regimen FOREIGN KEY (codigo_regimen_tributario)
        REFERENCES empresa.regimen_tributario (codigo) ON DELETE RESTRICT,
    CONSTRAINT uk_regimen_anio_activo UNIQUE (codigo_regimen_tributario, anio, activo)
);

CREATE INDEX IF NOT EXISTS idx_regimen_limite_anio_activo ON empresa.regimen_tributario_limite (anio, activo);

-- ----------------------------------------------------------------------------
-- Registros Iniciales Oficiales para el Año Fiscal 2026 (UIT = S/ 5,350.00)
-- ----------------------------------------------------------------------------
INSERT INTO empresa.regimen_tributario_limite (
    codigo_regimen_tributario, anio, valor_uit,
    limite_mensual_ventas, limite_mensual_compras,
    limite_anual_ventas, limite_anual_compras,
    limite_anual_ventas_uit,
    activo, creado_por, fecha_creacion
) VALUES
-- 1. Nuevo RUS (NRUS)
('NRUS', 2026, 5350.00, 8000.00, 8000.00, 96000.00, 96000.00, NULL, TRUE, 'CORE', CURRENT_TIMESTAMP),

-- 2. Régimen Especial de Renta (RER)
('RER', 2026, 5350.00, NULL, NULL, 525000.00, 525000.00, NULL, TRUE, 'CORE', CURRENT_TIMESTAMP),

-- 3. Régimen MYPE Tributario (RMT) - 1,700 UIT (S/ 9,095,000.00) en ventas / Compras sin límite
('RMT', 2026, 5350.00, NULL, NULL, 9095000.00, NULL, 1700, TRUE, 'CORE', CURRENT_TIMESTAMP),

-- 4. Régimen General (RG) - Sin límite de ventas ni compras
('RG', 2026, 5350.00, NULL, NULL, NULL, NULL, NULL, TRUE, 'CORE', CURRENT_TIMESTAMP)
ON CONFLICT (codigo_regimen_tributario, anio, activo) DO NOTHING;
