/* SCRIPT DE ATUALIZAÇÃO DO SFF*/

ALTER TABLE FAMILIA ADD COLUMN hora_criacao TimestampTz;
ALTER TABLE FAMILIA ADD COLUMN hora_atualizacao TimestampTz;

ALTER TABLE usuario ALTER COLUMN senha TYPE varchar(65);
ALTER TABLE usuario ADD COLUMN numero_falhas_login INTEGER;
ALTER TABLE usuario ADD COLUMN bloqueado_ate TimestampTz;
ALTER TABLE usuario ADD COLUMN hora_criacao TimestampTz;
ALTER TABLE usuario ADD COLUMN hora_atualizacao TimestampTz;

UPDATE usuario SET hora_criacao = NOW();
UPDATE usuario SET senha = '9600ec8be2bd9781c8c69d257ee50f40542a72b73943f9234175692bf261e625' where id = 1;
UPDATE usuario SET senha = 'e598901b629564f390f0a945f95054e79dbbb27b752b3825b86a46dfb3ce6f69' where id = 385;


CREATE SEQUENCE sessao_id_seq;

CREATE TABLE sessao(
	id bigint PRIMARY KEY DEFAULT nextval('sessao_id_seq'),
	ip varchar(15),
	data_expiracao TimestampTz,
	refresh_token varchar(50),
	data_expiracao_refresh_token TimestampTz,
	hora_criacao TimestampTz,
	hora_atualizacao TimestampTz
);

ALTER SEQUENCE sessao_id_seq
OWNED BY sessao.id;

ALTER TABLE sessao ADD COLUMN usuario_id bigint;
ALTER TABLE sessao ADD CONSTRAINT FK_sessao_usuario FOREIGN KEY(usuario_id) REFERENCES usuario(id);
