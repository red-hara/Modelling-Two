using Godot;

/// <summary>Helper class for geometry computation.</summary>
public static class Geometry
{
    /// <summary>Calculate points of intersection of three spheres of the same
    /// radius.</summary>
    /// <param name="a">The center of the first sphere.</param>
    /// <param name="b">The center of the second sphere.</param>
    /// <param name="c">The center of the third sphere.</param>
    /// <param name="radius">The radius of spheres</param>
    /// <returns>Up to two points of intersection. Zero points if no
    /// intersection is found.</returns>
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

    /// <summary>Calculate intersection of sphere and circle.</summary>
    /// <param name="radius">The sphere radius.</param>
    /// <param name="sphereCenter">The sphere center position.</param>
    /// <param name="circleCenter">The circle center position.</param>
    /// <param name="axisU">The first orthogonal unit axis of a circle.</param>
    /// <param name="axisV">The second orthogonal unit axis of a circle.</param>
    /// <param name="circleRadius">The circle radius.</param>
    /// <returns>Tuple of two intersection points or <c>null</c> if no
    /// intersection is found.</returns>
    public static (Vector3 PositionA, Vector3 PositionB)? SphereCircleIntersection(
        float radius,
        Vector3 sphereCenter,
        Vector3 circleCenter,
        Vector3 axisU,
        Vector3 axisV,
        float circleRadius
    )
    {
        var maybeSolution = SphereCircleIntersectionAngles(radius, sphereCenter, circleCenter, axisU, axisV, circleRadius);
        if (maybeSolution is null)
        {
            return null;
        }
        var solution = maybeSolution ?? default;

        Vector3 positionA = circleCenter + circleRadius * Mathf.Cos(solution.SolutionA) * axisU + circleRadius * Mathf.Sin(solution.SolutionA) * axisV;
        Vector3 positionB = circleCenter + circleRadius * Mathf.Cos(solution.SolutionB) * axisU + circleRadius * Mathf.Sin(solution.SolutionB) * axisV;

        return (positionA, positionB);
    }

    /// <summary>Calculate intersection of two spheres.</summary>
    /// <param name="a">The first sphere center.</param>
    /// <param name="b">The second sphere center.</param>
    /// <param name="radius">The radius of the spheres.</param>
    /// <returns>Tuple representing the intersection circle of <c>null</c> if no
    /// intersection is found.</returns>
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

    /// <summary>Calculate arbitrary orthogonal vector to provided one.</summary>
    /// <param name="vector">The vector to calculate orthogonal to.</summary>
    /// <returns>The orthogonal vector.</returns>
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

    /// <summary>Calculate angles of intersection of given sphere and circle.
    /// The angle calculation is performed from the first orthogonal
    /// axis.</summary>
    /// <param name="radius">The sphere radius.</param>
    /// <param name="sphereCenter">The sphere center position.</param>
    /// <param name="circleCenter">The circle center position.</param>
    /// <param name="axisU">The first orthogonal unit axis of a circle.</param>
    /// <param name="axisV">The second orthogonal unit axis of a circle.</param>
    /// <param name="circleRadius">The circle radius.</param>
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

    private static (float SolutionA, float SolutionB)? SolveEquation(float alpha, float beta, float gamma)
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