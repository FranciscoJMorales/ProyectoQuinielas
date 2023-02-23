namespace ProyectoQuinielas.Queries;

public static class qPools
{
    public static FormattableString GetAll(int? userid)
    {
        return $@"select p.name as Nombre, not p.public as Privada, q.Participantes, p.users_limit as Límite, u.username as Administrador from quinielas.Pools p
                JOIN (
                select COUNT(*) as Participantes, pool_id from quinielas.UsersPools
                group by pool_id
                ) q
                on p.id = q.pool_id
                JOIN quinielas.Users u on p.admin_id = u.id
                WHERE exists (select * from quinielas.UsersPools WHERE user_id = {userid}) AND p.active;";
    }
}
