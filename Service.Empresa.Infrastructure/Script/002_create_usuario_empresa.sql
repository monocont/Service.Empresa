-- ============================================================================
-- Service.Empresa - Tabla de propiedad/acceso usuario-empresa (multi-tenant)
-- Motor: PostgreSQL
-- ============================================================================

CREATE TABLE IF NOT EXISTS empresa.usuario_empresa (
    id_usuario_empresa UUID NOT NULL,
    id_usuario          UUID NOT NULL,   -- GUID del usuario (seguridad.usuario.id_usuario), llega en el claim NameIdentifier del JWT
    id_empresa          UUID NOT NULL,   -- FK -> empresa.empresa
    rol                 VARCHAR(15) NOT NULL DEFAULT 'DUENO',  -- DUENO | COLABORADOR
    creado_por          VARCHAR(150),
    fecha_creacion      TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modificado_por      VARCHAR(150),
    fecha_modificacion  TIMESTAMP,
    activo              BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT pk_usuario_empresa PRIMARY KEY (id_usuario_empresa),
    CONSTRAINT uk_usuario_empresa UNIQUE (id_usuario, id_empresa),
    CONSTRAINT fk_ue_empresa FOREIGN KEY (id_empresa)
        REFERENCES empresa.empresa (id_empresa) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_ue_usuario ON empresa.usuario_empresa (id_usuario);
CREATE INDEX IF NOT EXISTS idx_ue_empresa ON empresa.usuario_empresa (id_empresa);

-- ----------------------------------------------------------------------------
-- Migración de datos existentes: las empresas ya registradas quedan asignadas
-- a su creador (empresa.creado_por ya almacena el GUID del usuario).
-- En una instalación desde cero este INSERT no genera filas.
-- ----------------------------------------------------------------------------
INSERT INTO empresa.usuario_empresa (id_usuario_empresa, id_usuario, id_empresa, rol, creado_por, fecha_creacion, activo)
SELECT gen_random_uuid(), e.creado_por::uuid, e.id_empresa, 'DUENO', 'migracion', CURRENT_TIMESTAMP, TRUE
FROM empresa.empresa e
WHERE e.activo = TRUE AND e.creado_por IS NOT NULL
ON CONFLICT (id_usuario, id_empresa) DO NOTHING;
