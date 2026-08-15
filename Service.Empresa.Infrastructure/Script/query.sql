-- ============================================================================
-- Service.Empresa - Script de Creación de Esquema y Tablas
-- Motor: PostgreSQL
-- ============================================================================

CREATE SCHEMA IF NOT EXISTS empresa;

-- 1. TABLA MAESTRA: REGIMEN TRIBUTARIO
CREATE TABLE empresa.regimen_tributario (
    codigo VARCHAR(10) NOT NULL,
    descripcion VARCHAR(100) NOT NULL,
    creado_por VARCHAR(150),
    fecha_creacion TIMESTAMP,
    modificado_por VARCHAR(150),
    fecha_modificacion TIMESTAMP,
    activo BOOLEAN DEFAULT TRUE,
    CONSTRAINT pk_regimen_tributario PRIMARY KEY (codigo)
);

INSERT INTO empresa.regimen_tributario (codigo, descripcion, creado_por, fecha_creacion, activo) VALUES
('NRUS', 'Nuevo Régimen Único Simplificado', 'CORE', CURRENT_TIMESTAMP, TRUE),
('RER', 'Régimen Especial de Renta', 'CORE', CURRENT_TIMESTAMP, TRUE),
('RMT', 'Régimen MYPE Tributario', 'CORE', CURRENT_TIMESTAMP, TRUE),
('RG', 'Régimen General', 'CORE', CURRENT_TIMESTAMP, TRUE);

-- 2. TABLA MAESTRA: ESTADO CONTRIBUYENTE
CREATE TABLE empresa.estado_contribuyente (
    codigo VARCHAR(20) NOT NULL,
    descripcion VARCHAR(100) NOT NULL,
    creado_por VARCHAR(150),
    fecha_creacion TIMESTAMP,
    modificado_por VARCHAR(150),
    fecha_modificacion TIMESTAMP,
    activo BOOLEAN DEFAULT TRUE,
    CONSTRAINT pk_estado_contribuyente PRIMARY KEY (codigo)
);

INSERT INTO empresa.estado_contribuyente (codigo, descripcion, creado_por, fecha_creacion, activo) VALUES
('ACTIVO', 'Activo', 'CORE', CURRENT_TIMESTAMP, TRUE),
('INACTIVO', 'Inactivo', 'CORE', CURRENT_TIMESTAMP, TRUE),
('SUSPENSION_TEMPORAL', 'Suspensión Temporal', 'CORE', CURRENT_TIMESTAMP, TRUE);

-- 3. TABLA MAESTRA: CONDICIÓN CONTRIBUYENTE
CREATE TABLE empresa.condicion_contribuyente (
    codigo VARCHAR(20) NOT NULL,
    descripcion VARCHAR(100) NOT NULL,
    creado_por VARCHAR(150),
    fecha_creacion TIMESTAMP,
    modificado_por VARCHAR(150),
    fecha_modificacion TIMESTAMP,
    activo BOOLEAN DEFAULT TRUE,
    CONSTRAINT pk_condicion_contribuyente PRIMARY KEY (codigo)
);

INSERT INTO empresa.condicion_contribuyente (codigo, descripcion, creado_por, fecha_creacion, activo) VALUES
('HABIDO', 'Habido', 'CORE', CURRENT_TIMESTAMP, TRUE),
('NO_HABIDO', 'No Habido', 'CORE', CURRENT_TIMESTAMP, TRUE),
('NO_HALLADO', 'No Hallado', 'CORE', CURRENT_TIMESTAMP, TRUE);

-- 4. TABLA PRINCIPAL: EMPRESA
CREATE TABLE empresa.empresa (
    id_empresa UUID NOT NULL,
    ruc CHAR(11) NOT NULL,
    razon_social VARCHAR(255) NOT NULL,
    nombre_comercial VARCHAR(255),
    
    codigo_regimen_tributario VARCHAR(10) NOT NULL,
    codigo_estado_contribuyente VARCHAR(20) NOT NULL DEFAULT 'ACTIVO',
    codigo_condicion_contribuyente VARCHAR(20) NOT NULL DEFAULT 'HABIDO',
    
    direccion_fiscal TEXT NOT NULL,
    ubigeo CHAR(6) NOT NULL,
    moneda_base CHAR(3) DEFAULT 'PEN',
    logo_url TEXT,
    
    -- auditoria
    creado_por VARCHAR(150),
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    modificado_por VARCHAR(150),
    fecha_modificacion TIMESTAMP,
    activo BOOLEAN DEFAULT TRUE,
    
    CONSTRAINT pk_empresa PRIMARY KEY (id_empresa),
    CONSTRAINT fk_empresa_regimen FOREIGN KEY (codigo_regimen_tributario) 
        REFERENCES empresa.regimen_tributario (codigo),
    CONSTRAINT fk_empresa_estado FOREIGN KEY (codigo_estado_contribuyente) 
        REFERENCES empresa.estado_contribuyente (codigo),
    CONSTRAINT fk_empresa_condicion FOREIGN KEY (codigo_condicion_contribuyente) 
        REFERENCES empresa.condicion_contribuyente (codigo)
);

CREATE UNIQUE INDEX ix_empresa_ruc_usuario_activo 
    ON empresa.empresa (ruc, creado_por) WHERE activo = true;

CREATE INDEX idx_empresa_ruc ON empresa.empresa (ruc);

-- 5. TABLA: CREDENCIAL SUNAT BÁSICA
CREATE TABLE empresa.credencial_sunat (
    id_credencial UUID NOT NULL,
    id_empresa UUID NOT NULL,
    usuario_sol VARCHAR(50) NOT NULL,
    clave_sol VARCHAR(100) NOT NULL,
    
    -- auditoria
    creado_por VARCHAR(150),
    fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    modificado_por VARCHAR(150),
    fecha_modificacion TIMESTAMP,
    activo BOOLEAN DEFAULT TRUE,
    
    CONSTRAINT pk_credencial_sunat PRIMARY KEY (id_credencial),
    CONSTRAINT uk_credencial_sunat_empresa UNIQUE (id_empresa),
    CONSTRAINT fk_credencial_empresa FOREIGN KEY (id_empresa) 
        REFERENCES empresa.empresa (id_empresa) ON DELETE CASCADE
);