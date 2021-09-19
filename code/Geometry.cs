using Godot;

public static class Geometry
{
    public static Vector3[] SphereIntersection3(Vector3 a, Vector3 b, Vector3 c, float radius)
    {
        var maybeCircle = SphereIntersection2(a, b, radius);
        if (maybeCircle is null)
        {
            return new Vector3[0];
        }
        var circle = maybeCircle ?? default;

        var maybePosition = SphereCircleIntersection(radius, c, circle.Center, circle.AxisU, circle.AxisV, circle.Radius);
        if (maybePosition is null)
        {
            return new Vector3[0];
        }
        var position = maybePosition ?? default;

        return new Vector3[]{
            position.PositionA,
            position.PositionB,
        };
    }

    public static (Vector3 PositionA, Vector3 PositionB)? SphereCircleIntersection(
        float radius,
        Vector3 sphereCenter,
        Vector3 circleCenter,
        Vector3 AxisU,
        Vector3 AxisV,
        float circleRadius
    )
    {
        var maybeSolution = SphereCircleIntersectionAngles(radius, sphereCenter, circleCenter, AxisU, AxisV, circleRadius);
        if (maybeSolution is null)
        {
            return null;
        }
        var solution = maybeSolution ?? default;

        Vector3 positionA = circleCenter + circleRadius * Mathf.Cos(solution.SolutionA) * AxisU + circleRadius * Mathf.Sin(solution.SolutionA) * AxisV;
        Vector3 positionB = circleCenter + circleRadius * Mathf.Cos(solution.SolutionB) * AxisU + circleRadius * Mathf.Sin(solution.SolutionB) * AxisV;

        return (positionA, positionB);
    }

    public static (float Radius, Vector3 Center, Vector3 AxisV, Vector3 AxisU)? SphereIntersection2(Vector3 a, Vector3 b, float radius)
    {
        Vector3 ab = b - a;
        float halfDistance = ab.Length() / 2;
        if (halfDistance > radius)
        {
            return null;
        }
        Vector3 abNorm = ab.Normalized();
        Vector3 AxisV = OrthogonalVector(abNorm).Normalized();
        Vector3 AxisU = abNorm.Cross(AxisV).Normalized();
        Vector3 center = a + ab / 2;
        float circeRadius = Mathf.Sqrt(radius * radius - halfDistance * halfDistance);
        return (circeRadius, center, AxisV, AxisU);
    }

    public static Vector3 OrthogonalVector(Vector3 vector)
    {
        float a = vector.x;
        float b = vector.y;
        float c = vector.z;
        return new Vector3(
            b + c,
            c - a,
            -a - b
        );
    }

    public static (float SolutionA, float SolutionB)? SphereCircleIntersectionAngles(
        float radius,
        Vector3 sphereCenter,
        Vector3 circleCenter,
        Vector3 axisU,
        Vector3 axisV,
        float circleRadius
    )
    {
        Vector3 delta = circleCenter - sphereCenter;
        float gamma = radius * radius - delta.LengthSquared() - circleRadius * circleRadius;
        float alpha = 2 * delta.Dot(axisU) * circleRadius;
        float beta = 2 * delta.Dot(axisV) * circleRadius;

        return SolveEquation(alpha, beta, gamma);
    }

    public static (float SolutionA, float SolutionB)? SolveEquation(float alpha, float beta, float gamma)
    {
        float c = gamma - alpha;
        float b = -2 * beta;
        float a = gamma + alpha;
        float disc = b * b - 4 * a * c;
        if (disc < 0)
        {
            return null;
        }
        float tt0 = (-b + Mathf.Sqrt(disc)) / 2 / a;
        float tt1 = (-b - Mathf.Sqrt(disc)) / 2 / a;

        tt0 = 2 * Mathf.Atan(tt0);
        tt1 = 2 * Mathf.Atan(tt1);

        return (tt0, tt1);
    }
}