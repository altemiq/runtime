// -----------------------------------------------------------------------
// <copyright file="Matrix4x4D.Impl.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Numerics;

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Altemiq.Runtime.Intrinsics;

/// <content>
/// The Matrix3x2D implementation.
/// </content>
[System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming", Justification = "This is valid")]
public partial struct Matrix4x4D
{
    /*
    See Matrix4x4D.cs for an explanation of why this file/type exists

    Note that we use some particular patterns below, such as defining a result
    and assigning the fields directly rather than using the object initializer
    syntax. We do this because it saves roughly 8-bytes of IL per method which
    in turn helps improve inlining chances.
    */

    /// <summary>
    /// Gets this <see cref="Matrix3x2D"/> as and <see cref="Impl"/>.
    /// </summary>
    /// <returns>The <see cref="Impl"/>.</returns>
    [System.Diagnostics.CodeAnalysis.UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0102:Make member readonly", Justification = "Checked")]
    internal ref Impl AsImpl() => ref Unsafe.As<Matrix4x4D, Impl>(ref this);

    /// <summary>
    /// Gets this <see cref="Matrix3x2D"/> as and <see cref="Impl"/>.
    /// </summary>
    /// <returns>The <see cref="Impl" />.</returns>
    [System.Diagnostics.CodeAnalysis.UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal readonly ref readonly Impl AsROImpl() => ref Unsafe.As<Matrix4x4D, Impl>(ref Unsafe.AsRef(in this));

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass", Justification = "Checked")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3218:Inner class members should not shadow outer class \"static\" or type members", Justification = "Checked")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Checked")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1242:Do not pass non-read-only struct by read-only reference", Justification = "Checked")]
    internal struct Impl : IEquatable<Impl>
    {
        public Vector4D X;
        public Vector4D Y;
        public Vector4D Z;
        public Vector4D W;

        private const double BillboardEpsilon = 1e-4;
        private const double BillboardMinAngle = 1.0 - (0.1 * (double.Pi / 180.0)); // 0.1 degrees
        private const double DecomposeEpsilon = 0.0001;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl operator +(in Impl left, in Impl right)
        {
            Impl result;

            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Impl left, in Impl right) =>
            left.X == right.X
            && left.Y == right.Y
            && left.Z == right.Z
            && left.W == right.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Impl left, in Impl right) =>
            left.X != right.X
            || left.Y != right.Y
            || left.Z != right.Z
            || left.W != right.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl operator *(in Impl left, double right)
        {
            Impl result;

            result.X = left.X * right;
            result.Y = left.Y * right;
            result.Z = left.Z * right;
            result.W = left.W * right;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl operator -(in Impl left, in Impl right)
        {
            Impl result;

            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl operator -(in Impl value)
        {
            Impl result;

            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            result.W = -value.W;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateBillboard(in Vector3D objectPosition, in Vector3D cameraPosition, in Vector3D cameraUpVector, in Vector3D cameraForwardVector)
        {
            // In a right-handed coordinate system, the object's positive z-axis is in the opposite direction as its forward vector,
            // and spherical billboards by construction always face the camera.
            var axisZ = objectPosition - cameraPosition;

            // When object and camera position are approximately the same, the object should just face the
            // same direction as the camera is facing.
            if (axisZ.LengthSquared() < BillboardEpsilon)
            {
                axisZ = -cameraForwardVector;
            }
            else
            {
                axisZ = Vector3D.Normalize(axisZ);
            }

            var axisX = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, axisZ));
            var axisY = Vector3D.Cross(axisZ, axisX);

            Impl result;

            result.X = axisX.AsVector4D();
            result.Y = axisY.AsVector4D();
            result.Z = axisZ.AsVector4D();
            result.W = Vector4D.Create(objectPosition, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateBillboardLeftHanded(in Vector3D objectPosition, in Vector3D cameraPosition, in Vector3D cameraUpVector, in Vector3D cameraForwardVector)
        {
            // In a left-handed coordinate system, the object's positive z-axis is in the same direction as its forward vector,
            // and spherical billboards by construction always face the camera.
            var axisZ = cameraPosition - objectPosition;

            // When object and camera position are approximately the same, the object should just face the
            // same direction as the camera is facing.
            axisZ = axisZ.LengthSquared() < BillboardEpsilon ? cameraForwardVector : Vector3D.Normalize(axisZ);

            var axisX = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, axisZ));
            var axisY = Vector3D.Cross(axisZ, axisX);

            Impl result;

            result.X = axisX.AsVector4D();
            result.Y = axisY.AsVector4D();
            result.Z = axisZ.AsVector4D();
            result.W = Vector4D.Create(objectPosition, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateConstrainedBillboard(in Vector3D objectPosition, in Vector3D cameraPosition, in Vector3D rotateAxis, in Vector3D cameraForwardVector, in Vector3D objectForwardVector)
        {
            // First find the Z-axis of the spherical/unconstrained rotation. We call this faceDir and in a right-handed coordinate system
            // it will be in the opposite direction as from the object to the camera.
            var faceDir = objectPosition - cameraPosition;

            // When object and camera position are approximately the same this indicates that the object should also just face the
            // same direction as the camera is facing.
            if (faceDir.LengthSquared() < BillboardEpsilon)
            {
                faceDir = -cameraForwardVector;
            }
            else
            {
                faceDir = Vector3D.Normalize(faceDir);
            }

            var axisY = rotateAxis;

            var dot = Vector3D.Dot(axisY, faceDir);

            // Generally the approximation for small angles is cos theta = 1 - theta^2 / 2,
            // but it seems that here we are using cos theta = 1 - theta. Letting theta be the angle
            // between the rotate axis and the faceDir,
            //
            // dot = cos theta ~ 1 - theta > 1 - .1 * pi/180 = 1 - (.1 degree) => theta < .1 degree
            //
            // So this condition checks if the faceDir is approximately the same as the rotate axis
            // by checking if the angle between them is less than .1 degree.
            if (double.Abs(dot) > BillboardMinAngle)
            {
                // If the faceDir is approximately the same as the rotate axis, then fallback to using object forward vector
                // as the faceDir.
                faceDir = objectForwardVector;

                dot = Vector3D.Dot(axisY, faceDir);

                // Similar to before, check if the faceDir is still is approximately the rotate axis.
                // If so, then use either -UnitZ or UnitX as the fallback faceDir.
                if (double.Abs(dot) > BillboardMinAngle)
                {
                    // |axisY.Z| = |dot(axisY, -UnitZ)|, so this is checking if the rotate axis is approximately the same as -UnitZ.
                    // If is, then use UnitX as the fallback.
                    faceDir = double.Abs(axisY.Z) > BillboardMinAngle ? Vector3D.UnitX : Vector3D.Create(0, 0, -1);
                }
            }

            var axisX = Vector3D.Normalize(Vector3D.Cross(axisY, faceDir));
            var axisZ = Vector3D.Normalize(Vector3D.Cross(axisX, axisY));

            Impl result;

            result.X = axisX.AsVector4D();
            result.Y = axisY.AsVector4D();
            result.Z = axisZ.AsVector4D();
            result.W = Vector4D.Create(objectPosition, 1);

            return result;
        }

        public static Impl CreateConstrainedBillboardLeftHanded(in Vector3D objectPosition, in Vector3D cameraPosition, in Vector3D rotateAxis, in Vector3D cameraForwardVector, in Vector3D objectForwardVector)
        {
            // First find the Z-axis of the spherical/unconstrained rotation. We call this faceDir and in a left-handed coordinate system
            // it will be in the same direction as from the object to the camera.
            var faceDir = cameraPosition - objectPosition;

            // When object and camera position are approximately the same this indicates that the object should also just face the
            // same direction as the camera is facing.
            faceDir = faceDir.LengthSquared() < BillboardEpsilon ? cameraForwardVector : Vector3D.Normalize(faceDir);

            var axisY = rotateAxis;

            var dot = Vector3D.Dot(axisY, faceDir);

            // Generally the approximation for small angles is cos theta = 1 - theta^2 / 2,
            // but it seems that here we are using cos theta = 1 - theta. Letting theta be the angle
            // between the rotate axis and the faceDir,
            //
            // dot = cos theta ~ 1 - theta > 1 - .1 * pi/180 = 1 - (.1 degree) => theta < .1 degree
            //
            // So this condition checks if the faceDir is approximately the same as the rotate axis
            // by checking if the angle between them is less than .1 degree.
            if (double.Abs(dot) > BillboardMinAngle)
            {
                // If the faceDir is approximately the same as the rotate axis, then fallback to using object forward vector
                // as the faceDir.
                faceDir = -objectForwardVector;

                dot = Vector3D.Dot(axisY, faceDir);

                // Similar to before, check if the faceDir is still is approximately the rotate axis.
                // If so, then use either -UnitZ or -UnitX as the fallback faceDir.
                if (double.Abs(dot) > BillboardMinAngle)
                {
                    // |axisY.Z| = |dot(axisY, -UnitZ)|, so this is checking if the rotate axis is approximately the same as -UnitZ.
                    // If is, then use -UnitX as the fallback.
                    faceDir = double.Abs(axisY.Z) > BillboardMinAngle ? Vector3D.Create(-1, 0, 0) : Vector3D.Create(0, 0, -1);
                }
            }

            var axisX = Vector3D.Normalize(Vector3D.Cross(axisY, faceDir));
            var axisZ = Vector3D.Normalize(Vector3D.Cross(axisX, axisY));

            Impl result;

            result.X = axisX.AsVector4D();
            result.Y = axisY.AsVector4D();
            result.Z = axisZ.AsVector4D();
            result.W = Vector4D.Create(objectPosition, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateFromAxisAngle(in Vector3D axis, double angle)
        {
            var q = QuaternionD.CreateFromAxisAngle(axis, angle);
            return CreateFromQuaternion(q);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateFromQuaternion(in QuaternionD quaternion)
        {
            var xx = quaternion.X * quaternion.X;
            var yy = quaternion.Y * quaternion.Y;
            var zz = quaternion.Z * quaternion.Z;

            var xy = quaternion.X * quaternion.Y;
            var wz = quaternion.Z * quaternion.W;
            var xz = quaternion.Z * quaternion.X;
            var wy = quaternion.Y * quaternion.W;
            var yz = quaternion.Y * quaternion.Z;
            var wx = quaternion.X * quaternion.W;

            Impl result;

            result.X = Vector4D.Create(
                1.0 - (2.0 * (yy + zz)),
                2.0 * (xy + wz),
                2.0 * (xz - wy),
                0);
            result.Y = Vector4D.Create(
                2.0 * (xy - wz),
                1.0 - (2.0 * (zz + xx)),
                2.0 * (yz + wx),
                0);
            result.Z = Vector4D.Create(
                2.0 * (xz + wy),
                2.0 * (yz - wx),
                1.0 - (2.0 * (yy + xx)),
                0);
            result.W = Vector4D.UnitW;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateFromYawPitchRoll(double yaw, double pitch, double roll)
        {
            var q = QuaternionD.CreateFromYawPitchRoll(yaw, pitch, roll);
            return CreateFromQuaternion(q);
        }

        // This implementation is based on the DirectX Math Library XMMatrixLookToRH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateLookTo(in Vector3D cameraPosition, in Vector3D cameraDirection, in Vector3D cameraUpVector) =>
            CreateLookToLeftHanded(cameraPosition, -cameraDirection, cameraUpVector);

        // This implementation is based on the DirectX Math Library XMMatrixLookToLH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateLookToLeftHanded(in Vector3D cameraPosition, in Vector3D cameraDirection, in Vector3D cameraUpVector)
        {
            var axisZ = Vector3D.Normalize(cameraDirection);
            var axisX = Vector3D.Normalize(Vector3D.Cross(cameraUpVector, axisZ));
            var axisY = Vector3D.Cross(axisZ, axisX);
            var negativeCameraPosition = -cameraPosition;

            Impl result;

            result.X = Vector4D.Create(axisX, Vector3D.Dot(axisX, negativeCameraPosition));
            result.Y = Vector4D.Create(axisY, Vector3D.Dot(axisY, negativeCameraPosition));
            result.Z = Vector4D.Create(axisZ, Vector3D.Dot(axisZ, negativeCameraPosition));
            result.W = Vector4D.UnitW;

            return Transpose(result);
        }

        // This implementation is based on the DirectX Math Library XMMatrixOrthographicRH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateOrthographic(double width, double height, double zNearPlane, double zFarPlane)
        {
            var range = 1.0 / (zNearPlane - zFarPlane);

            Impl result;

            result.X = Vector4D.Create(2.0 / width, 0, 0, 0);
            result.Y = Vector4D.Create(0, 2.0 / height, 0, 0);
            result.Z = Vector4D.Create(0, 0, range, 0);
            result.W = Vector4D.Create(0, 0, range * zNearPlane, 1);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixOrthographicLH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateOrthographicLeftHanded(double width, double height, double zNearPlane, double zFarPlane)
        {
            var range = 1.0 / (zFarPlane - zNearPlane);

            Impl result;

            result.X = Vector4D.Create(2.0 / width, 0, 0, 0);
            result.Y = Vector4D.Create(0, 2.0 / height, 0, 0);
            result.Z = Vector4D.Create(0, 0, range, 0);
            result.W = Vector4D.Create(0, 0, -range * zNearPlane, 1);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixOrthographicOffCenterRH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateOrthographicOffCenter(double left, double right, double bottom, double top, double zNearPlane, double zFarPlane)
        {
            var reciprocalWidth = 1.0 / (right - left);
            var reciprocalHeight = 1.0 / (top - bottom);
            var range = 1.0 / (zNearPlane - zFarPlane);

            Impl result;

            result.X = Vector4D.Create(reciprocalWidth + reciprocalWidth, 0, 0, 0);
            result.Y = Vector4D.Create(0, reciprocalHeight + reciprocalHeight, 0, 0);
            result.Z = Vector4D.Create(0, 0, range, 0);
            result.W = Vector4D.Create(
                -(left + right) * reciprocalWidth,
                -(top + bottom) * reciprocalHeight,
                range * zNearPlane,
                1);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixOrthographicOffCenterLH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateOrthographicOffCenterLeftHanded(double left, double right, double bottom, double top, double zNearPlane, double zFarPlane)
        {
            var reciprocalWidth = 1.0 / (right - left);
            var reciprocalHeight = 1.0 / (top - bottom);
            var range = 1.0 / (zFarPlane - zNearPlane);

            Impl result;

            result.X = Vector4D.Create(reciprocalWidth + reciprocalWidth, 0, 0, 0);
            result.Y = Vector4D.Create(0, reciprocalHeight + reciprocalHeight, 0, 0);
            result.Z = Vector4D.Create(0, 0, range, 0);
            result.W = Vector4D.Create(
                -(left + right) * reciprocalWidth,
                -(top + bottom) * reciprocalHeight,
                -range * zNearPlane,
                1);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixPerspectiveRH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreatePerspective(double width, double height, double nearPlaneDistance, double farPlaneDistance)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

            var dblNearPlaneDistance = nearPlaneDistance + nearPlaneDistance;
            var range = double.IsPositiveInfinity(farPlaneDistance) ? -1.0 : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

            Impl result;

            result.X = Vector4D.Create(dblNearPlaneDistance / width, 0, 0, 0);
            result.Y = Vector4D.Create(0, dblNearPlaneDistance / height, 0, 0);
            result.Z = Vector4D.Create(0, 0, range, -1.0);
            result.W = Vector4D.Create(0, 0, range * nearPlaneDistance, 0);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixPerspectiveLH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreatePerspectiveLeftHanded(double width, double height, double nearPlaneDistance, double farPlaneDistance)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

            var dblNearPlaneDistance = nearPlaneDistance + nearPlaneDistance;
            var range = double.IsPositiveInfinity(farPlaneDistance) ? 1.0 : farPlaneDistance / (farPlaneDistance - nearPlaneDistance);

            Impl result;

            result.X = Vector4D.Create(dblNearPlaneDistance / width, 0, 0, 0);
            result.Y = Vector4D.Create(0, dblNearPlaneDistance / height, 0, 0);
            result.Z = Vector4D.Create(0, 0, range, 1.0);
            result.W = Vector4D.Create(0, 0, -range * nearPlaneDistance, 0);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixPerspectiveFovRH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreatePerspectiveFieldOfView(double fieldOfView, double aspectRatio, double nearPlaneDistance, double farPlaneDistance)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(fieldOfView, 0.0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(fieldOfView, double.Pi);

            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

            var height = 1.0 / double.Tan(fieldOfView * 0.5);
            var width = height / aspectRatio;
            var range = double.IsPositiveInfinity(farPlaneDistance) ? -1.0 : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

            Impl result;

            result.X = Vector4D.Create(width, 0, 0, 0);
            result.Y = Vector4D.Create(0, height, 0, 0);
            result.Z = Vector4D.Create(0, 0, range, -1.0);
            result.W = Vector4D.Create(0, 0, range * nearPlaneDistance, 0);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixPerspectiveFovLH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreatePerspectiveFieldOfViewLeftHanded(double fieldOfView, double aspectRatio, double nearPlaneDistance, double farPlaneDistance)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(fieldOfView, 0.0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(fieldOfView, double.Pi);

            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

            var height = 1.0 / double.Tan(fieldOfView * 0.5);
            var width = height / aspectRatio;
            var range = double.IsPositiveInfinity(farPlaneDistance) ? 1.0 : farPlaneDistance / (farPlaneDistance - nearPlaneDistance);

            Impl result;

            result.X = Vector4D.Create(width, 0, 0, 0);
            result.Y = Vector4D.Create(0, height, 0, 0);
            result.Z = Vector4D.Create(0, 0, range, 1.0);
            result.W = Vector4D.Create(0, 0, -range * nearPlaneDistance, 0);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixPerspectiveOffCenterRH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreatePerspectiveOffCenter(double left, double right, double bottom, double top, double nearPlaneDistance, double farPlaneDistance)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

            var dblNearPlaneDistance = nearPlaneDistance + nearPlaneDistance;
            var reciprocalWidth = 1.0 / (right - left);
            var reciprocalHeight = 1.0 / (top - bottom);
            var range = double.IsPositiveInfinity(farPlaneDistance) ? -1.0 : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

            Impl result;

            result.X = Vector4D.Create(dblNearPlaneDistance * reciprocalWidth, 0, 0, 0);
            result.Y = Vector4D.Create(0, dblNearPlaneDistance * reciprocalHeight, 0, 0);
            result.Z = Vector4D.Create(
                (left + right) * reciprocalWidth,
                (top + bottom) * reciprocalHeight,
                range,
                -1.0);
            result.W = Vector4D.Create(0, 0, range * nearPlaneDistance, 0);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixPerspectiveOffCenterLH method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreatePerspectiveOffCenterLeftHanded(double left, double right, double bottom, double top, double nearPlaneDistance, double farPlaneDistance)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(nearPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(farPlaneDistance, 0.0);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(nearPlaneDistance, farPlaneDistance);

            var dblNearPlaneDistance = nearPlaneDistance + nearPlaneDistance;
            var reciprocalWidth = 1.0 / (right - left);
            var reciprocalHeight = 1.0 / (top - bottom);
            var range = double.IsPositiveInfinity(farPlaneDistance) ? 1.0 : farPlaneDistance / (farPlaneDistance - nearPlaneDistance);

            Impl result;

            result.X = Vector4D.Create(dblNearPlaneDistance * reciprocalWidth, 0, 0, 0);
            result.Y = Vector4D.Create(0, dblNearPlaneDistance * reciprocalHeight, 0, 0);
            result.Z = Vector4D.Create(
                -(left + right) * reciprocalWidth,
                -(top + bottom) * reciprocalHeight,
                range,
                1.0);
            result.W = Vector4D.Create(0, 0, -range * nearPlaneDistance, 0);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixReflect method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateReflection(in PlaneD value)
        {
            var p = PlaneD.Normalize(value).AsVector4D();
            var s = p * Vector4D.Create(-2.0, -2.0, -2.0, 0.0);

            Impl result;

            result.X = Vector4D.MultiplyAddEstimate(Vector4D.Create(p.X), s, Vector4D.UnitX);
            result.Y = Vector4D.MultiplyAddEstimate(Vector4D.Create(p.Y), s, Vector4D.UnitY);
            result.Z = Vector4D.MultiplyAddEstimate(Vector4D.Create(p.Z), s, Vector4D.UnitZ);
            result.W = Vector4D.MultiplyAddEstimate(Vector4D.Create(p.W), s, Vector4D.UnitW);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateRotationX(double radians)
        {
            var (s, c) = double.SinCos(radians);

            // [  1  0  0  0 ]
            // [  0  c  s  0 ]
            // [  0 -s  c  0 ]
            // [  0  0  0  1 ]
            Impl result;

            result.X = Vector4D.UnitX;
            result.Y = Vector4D.Create(0, c, s, 0);
            result.Z = Vector4D.Create(0, -s, c, 0);
            result.W = Vector4D.UnitW;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateRotationX(double radians, in Vector3D centerPoint)
        {
            var (s, c) = double.SinCos(radians);

#if NET9_0_OR_GREATER
            var y = double.MultiplyAddEstimate(centerPoint.Y, 1 - c, +centerPoint.Z * s);
            var z = double.MultiplyAddEstimate(centerPoint.Z, 1 - c, -centerPoint.Y * s);
#else
            var y = (centerPoint.Y * (1 - c)) + (centerPoint.Z * s);
            var z = (centerPoint.Z * (1 - c)) - (centerPoint.Y * s);
#endif

            // [  1  0  0  0 ]
            // [  0  c  s  0 ]
            // [  0 -s  c  0 ]
            // [  0  y  z  1 ]
            Impl result;

            result.X = Vector4D.UnitX;
            result.Y = Vector4D.Create(0, c, s, 0);
            result.Z = Vector4D.Create(0, -s, c, 0);
            result.W = Vector4D.Create(0, y, z, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateRotationY(double radians)
        {
            var (s, c) = double.SinCos(radians);

            // [  c  0 -s  0 ]
            // [  0  1  0  0 ]
            // [  s  0  c  0 ]
            // [  0  0  0  1 ]
            Impl result;

            result.X = Vector4D.Create(c, 0, -s, 0);
            result.Y = Vector4D.UnitY;
            result.Z = Vector4D.Create(s, 0, c, 0);
            result.W = Vector4D.UnitW;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateRotationY(double radians, in Vector3D centerPoint)
        {
            var (s, c) = double.SinCos(radians);

#if NET9_0_OR_GREATER
            var x = double.MultiplyAddEstimate(centerPoint.X, 1 - c, -centerPoint.Z * s);
            var z = double.MultiplyAddEstimate(centerPoint.Z, 1 - c, +centerPoint.X * s);
#else
            var x = (centerPoint.X * (1 - c)) - (centerPoint.Z * s);
            var z = (centerPoint.Z * (1 - c)) + (centerPoint.X * s);
#endif

            // [  c  0 -s  0 ]
            // [  0  1  0  0 ]
            // [  s  0  c  0 ]
            // [  x  0  z  1 ]
            Impl result;

            result.X = Vector4D.Create(c, 0, -s, 0);
            result.Y = Vector4D.UnitY;
            result.Z = Vector4D.Create(s, 0, c, 0);
            result.W = Vector4D.Create(x, 0, z, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateRotationZ(double radians)
        {
            var (s, c) = double.SinCos(radians);

            // [  c  s  0  0 ]
            // [ -s  c  0  0 ]
            // [  0  0  1  0 ]
            // [  0  0  0  1 ]
            Impl result;

            result.X = Vector4D.Create(c, s, 0, 0);
            result.Y = Vector4D.Create(-s, c, 0, 0);
            result.Z = Vector4D.UnitZ;
            result.W = Vector4D.UnitW;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateRotationZ(double radians, in Vector3D centerPoint)
        {
            var (s, c) = double.SinCos(radians);

#if NET9_0_OR_GREATER
            var x = double.MultiplyAddEstimate(centerPoint.X, 1 - c, +centerPoint.Y * s);
            var y = double.MultiplyAddEstimate(centerPoint.Y, 1 - c, -centerPoint.X * s);
#else
            var x = (centerPoint.X * (1 - c)) + (centerPoint.Y * s);
            var y = (centerPoint.Y * (1 - c)) - (centerPoint.X * s);
#endif

            // [  c  s  0  0 ]
            // [ -s  c  0  0 ]
            // [  0  0  1  0 ]
            // [  x  y  0  1 ]
            Impl result;

            result.X = Vector4D.Create(c, s, 0, 0);
            result.Y = Vector4D.Create(-s, c, 0, 0);
            result.Z = Vector4D.UnitZ;
            result.W = Vector4D.Create(x, y, 0, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateScale(double scaleX, double scaleY, double scaleZ)
        {
            Impl result;

            result.X = Vector4D.Create(scaleX, 0, 0, 0);
            result.Y = Vector4D.Create(0, scaleY, 0, 0);
            result.Z = Vector4D.Create(0, 0, scaleZ, 0);
            result.W = Vector4D.UnitW;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateScale(double scaleX, double scaleY, double scaleZ, in Vector3D centerPoint)
        {
            Impl result;

            result.X = Vector4D.Create(scaleX, 0, 0, 0);
            result.Y = Vector4D.Create(0, scaleY, 0, 0);
            result.Z = Vector4D.Create(0, 0, scaleZ, 0);
            result.W = Vector4D.Create(centerPoint * (Vector3D.One - Vector3D.Create(scaleX, scaleY, scaleZ)), 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateScale(in Vector3D scales)
        {
            Impl result;

            result.X = Vector4D.Create(scales.X, 0, 0, 0);
            result.Y = Vector4D.Create(0, scales.Y, 0, 0);
            result.Z = Vector4D.Create(0, 0, scales.Z, 0);
            result.W = Vector4D.UnitW;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateScale(in Vector3D scales, in Vector3D centerPoint)
        {
            Impl result;

            result.X = Vector4D.Create(scales.X, 0, 0, 0);
            result.Y = Vector4D.Create(0, scales.Y, 0, 0);
            result.Z = Vector4D.Create(0, 0, scales.Z, 0);
            result.W = Vector4D.Create(centerPoint * (Vector3D.One - scales), 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateScale(double scale)
        {
            Impl result;

            result.X = Vector4D.Create(scale, 0, 0, 0);
            result.Y = Vector4D.Create(0, scale, 0, 0);
            result.Z = Vector4D.Create(0, 0, scale, 0);
            result.W = Vector4D.UnitW;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateScale(double scale, in Vector3D centerPoint)
        {
            Impl result;

            result.X = Vector4D.Create(scale, 0, 0, 0);
            result.Y = Vector4D.Create(0, scale, 0, 0);
            result.Z = Vector4D.Create(0, 0, scale, 0);
            result.W = Vector4D.Create(centerPoint * (Vector3D.One - Vector3D.Create(scale)), 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateShadow(in Vector3D lightDirection, in PlaneD plane)
        {
            var p = PlaneD.Normalize(plane).AsVector4D();
            var l = lightDirection.AsVector4D();
            var dot = Vector4D.Dot(p, l);

            p = -p;

            Impl result;

            result.X = Vector4D.MultiplyAddEstimate(l, Vector4D.Create(p.X), Vector4D.Create(dot, 0, 0, 0));
            result.Y = Vector4D.MultiplyAddEstimate(l, Vector4D.Create(p.Y), Vector4D.Create(0, dot, 0, 0));
            result.Z = Vector4D.MultiplyAddEstimate(l, Vector4D.Create(p.Z), Vector4D.Create(0, 0, dot, 0));
            result.W = Vector4D.MultiplyAddEstimate(l, Vector4D.Create(p.W), Vector4D.Create(0, 0, 0, dot));

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateTranslation(in Vector3D position)
        {
            Impl result;

            result.X = Vector4D.UnitX;
            result.Y = Vector4D.UnitY;
            result.Z = Vector4D.UnitZ;
            result.W = Vector4D.Create(position, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateTranslation(double positionX, double positionY, double positionZ)
        {
            Impl result;

            result.X = Vector4D.UnitX;
            result.Y = Vector4D.UnitY;
            result.Z = Vector4D.UnitZ;
            result.W = Vector4D.Create(positionX, positionY, positionZ, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateViewport(double x, double y, double width, double height, double minDepth, double maxDepth)
        {
            Impl result;

            // 4x SIMD fields to get a lot better codegen
            result.W = Vector4D.Create(width, height, 0, 0);
            result.W *= Vector4D.Create(0.5, 0.5, 0, 0);

            result.X = Vector4D.Create(result.W.X, 0, 0, 0);
            result.Y = Vector4D.Create(0, -result.W.Y, 0, 0);
            result.Z = Vector4D.Create(0, 0, minDepth - maxDepth, 0);
            result.W += Vector4D.Create(x, y, minDepth, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateViewportLeftHanded(double x, double y, double width, double height, double minDepth, double maxDepth)
        {
            Impl result;

            // 4x SIMD fields to get a lot better codegen
            result.W = Vector4D.Create(width, height, 0, 0);
            result.W *= Vector4D.Create(0.5, 0.5, 0, 0);

            result.X = Vector4D.Create(result.W.X, 0, 0, 0);
            result.Y = Vector4D.Create(0, -result.W.Y, 0, 0);
            result.Z = Vector4D.Create(0, 0, maxDepth - minDepth, 0);
            result.W += Vector4D.Create(x, y, minDepth, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl CreateWorld(in Vector3D position, in Vector3D forward, in Vector3D up)
        {
            var axisZ = Vector3D.Normalize(-forward);
            var axisX = Vector3D.Normalize(Vector3D.Cross(up, axisZ));
            var axisY = Vector3D.Cross(axisZ, axisX);

            Impl result;

            result.X = axisX.AsVector4D();
            result.Y = axisY.AsVector4D();
            result.Z = axisZ.AsVector4D();
            result.W = Vector4D.Create(position, 1);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Decompose(in Impl matrix, out Vector3D scale, out QuaternionD rotation, out Vector3D translation)
        {
            var matTemp = Identity.AsImpl();

            var canonicalBasis = stackalloc Vector3D[3] { Vector3D.UnitX, Vector3D.UnitY, Vector3D.UnitZ, };

            translation = matrix.W.AsVector3D();

            var vectorBasis = stackalloc Vector3D*[3] { (Vector3D*)&matTemp.X, (Vector3D*)&matTemp.Y, (Vector3D*)&matTemp.Z, };

            *vectorBasis[0] = matrix.X.AsVector3D();
            *vectorBasis[1] = matrix.Y.AsVector3D();
            *vectorBasis[2] = matrix.Z.AsVector3D();

            var scales = stackalloc double[3] { vectorBasis[0]->Length(), vectorBasis[1]->Length(), vectorBasis[2]->Length(), };

            uint a, b, c;
            var x = scales[0];
            var y = scales[1];
            var z = scales[2];

            if (x < y)
            {
                if (y < z)
                {
                    a = 2;
                    b = 1;
                    c = 0;
                }
                else
                {
                    a = 1;

                    if (x < z)
                    {
                        b = 2;
                        c = 0;
                    }
                    else
                    {
                        b = 0;
                        c = 2;
                    }
                }
            }
            else
            {
                if (x < z)
                {
                    a = 2;
                    b = 0;
                    c = 1;
                }
                else
                {
                    a = 0;

                    if (y < z)
                    {
                        b = 2;
                        c = 1;
                    }
                    else
                    {
                        b = 1;
                        c = 2;
                    }
                }
            }

            if (scales[a] < DecomposeEpsilon)
            {
                *vectorBasis[a] = canonicalBasis[a];
            }

            *vectorBasis[a] = Vector3D.Normalize(*vectorBasis[a]);

            if (scales[b] < DecomposeEpsilon)
            {
                uint cc;

                var fAbsX = double.Abs(vectorBasis[a]->X);
                var fAbsY = double.Abs(vectorBasis[a]->Y);
                var fAbsZ = double.Abs(vectorBasis[a]->Z);
                if (fAbsX < fAbsY)
                {
                    if (fAbsY < fAbsZ)
                    {
                        cc = 0U;
                    }
                    else
                    {
                        cc = fAbsX < fAbsZ ? 0U : 2U;
                    }
                }
                else
                {
                    if (fAbsX < fAbsZ)
                    {
                        cc = 1U;
                    }
                    else
                    {
                        cc = fAbsY < fAbsZ ? 1U : 2U;
                    }
                }

                *vectorBasis[b] = Vector3D.Cross(*vectorBasis[a], canonicalBasis[cc]);
            }

            *vectorBasis[b] = Vector3D.Normalize(*vectorBasis[b]);

            if (scales[c] < DecomposeEpsilon)
            {
                *vectorBasis[c] = Vector3D.Cross(*vectorBasis[a], *vectorBasis[b]);
            }

            *vectorBasis[c] = Vector3D.Normalize(*vectorBasis[c]);

            var det = matTemp.GetDeterminant();

            // use Kramer's rule to check for handedness of coordinate system
            if (det < 0.0)
            {
                // switch coordinate system by negating the scale and inverting the basis vector on the x-axis
                scales[a] = -scales[a];
                *vectorBasis[a] = -(*vectorBasis[a]);

                det = -det;
            }

            det -= 1.0;
            det *= det;

            bool result;

            if (det > DecomposeEpsilon)
            {
                // Non-SRT matrix encountered
                rotation = QuaternionD.Identity;
                result = false;
            }
            else
            {
                // generate the quaternion from the matrix
                rotation = QuaternionD.CreateFromRotationMatrix(matTemp.AsM4x4D());
                result = true;
            }

            scale = Unsafe.ReadUnaligned<Vector3D>(scales);
            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixInverse method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Invert(in Impl matrix, out Impl result)
        {
            return Avx2.IsSupported
                ? AvxImpl(in matrix, out result)
                : SoftwareFallback(in matrix, out result);

            [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1312:Variable names should begin with lower-case letter", Justification = "Checked")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Checked")]
            static bool AvxImpl(in Impl matrix, out Impl result)
            {
                if (!Avx2.IsSupported)
                {
                    // Redundant test so we won't pre-jit remainder of this method on platforms without AVX.
                    ThrowPlatformNotSupportedException();
                }

                // Load the matrix values into rows
                var row1 = matrix.X.AsVector256();
                var row2 = matrix.Y.AsVector256();
                var row3 = matrix.Z.AsVector256();
                var row4 = matrix.W.AsVector256();

                // Transpose the matrix
                var vTemp1 = Avx.UnpackLow(row1, row2); // x[0], z[0], x[2], z[2]
                var vTemp3 = Avx.UnpackLow(row3, row4); // y[0], w[0], y[2], w[2]
                var vTemp2 = Avx.UnpackHigh(row1, row2); // x[1], z[1], x[4], z[4]
                var vTemp4 = Avx.UnpackHigh(row3, row4); // y[1], w[1], y[4], w[4]

                row1 = Avx.InsertVector128(vTemp1, vTemp3.GetLower(), 1); // x[0], y[0], z[0], w[0]
                row2 = Avx.InsertVector128(vTemp2, vTemp4.GetLower(), 1); // x[1], y[1], z[1], w[1]
                row3 = Avx.Permute2x128(vTemp1, vTemp3, 0b_00_11_00_01); // x[2], y[2], z[2], w[2]
                row4 = Avx.Permute2x128(vTemp2, vTemp4, 0b_00_11_00_01); // x[3], y[3], z[3], w[3]

                var V00 = Vector256.Shuffle(row3, Vector256.Create(0, 0, 1, 1));
                var V10 = Vector256.Shuffle(row4, Vector256.Create(2, 3, 2, 3));
                var V01 = Vector256.Shuffle(row1, Vector256.Create(0, 0, 1, 1));
                var V11 = Vector256.Shuffle(row2, Vector256.Create(2, 3, 2, 3));
                var V02 = Avx.Blend(Avx2.Permute4x64(row3, 0b10_00_10_00), Avx2.Permute4x64(row1, 0b10_00_10_00), 0b1100);
                var V12 = Avx.Blend(Avx2.Permute4x64(row4, 0b11_01_11_01), Avx2.Permute4x64(row2, 0b11_01_11_01), 0b1100);

                var D0 = V00 * V10;
                var D1 = V01 * V11;
                var D2 = V02 * V12;

                V00 = Vector256.Shuffle(row3, Vector256.Create(2, 3, 2, 3));
                V10 = Vector256.Shuffle(row4, Vector256.Create(0, 0, 1, 1));
                V01 = Vector256.Shuffle(row1, Vector256.Create(2, 3, 2, 3));
                V11 = Vector256.Shuffle(row2, Vector256.Create(0, 0, 1, 1));
                V02 = Avx.Blend(Avx2.Permute4x64(row3, 0b11_01_11_01), Avx2.Permute4x64(row1, 0b11_01_11_01), 0b1100);
                V12 = Avx.Blend(Avx2.Permute4x64(row4, 0b10_00_10_00), Avx2.Permute4x64(row2, 0b10_00_10_00), 0b1100);

                D0 = Vector256.MultiplyAddEstimate(-V00, V10, D0);
                D1 = Vector256.MultiplyAddEstimate(-V01, V11, D1);
                D2 = Vector256.MultiplyAddEstimate(-V02, V12, D2);

                // V11 = D0Y,D0W,D2Y,D2Y
                V11 = Avx.Blend(Avx2.Permute4x64(D0, 0b01_01_11_01), Avx2.Permute4x64(D2, 0b01_01_11_01), 0b1100);
                V00 = Vector256.Shuffle(row2, Vector256.Create(1, 2, 0, 1));
                V10 = Avx.Blend(Avx2.Permute4x64(V11, 0b00_11_00_10), Avx2.Permute4x64(D0, 0b00_11_00_10), 0b1100);
                V01 = Vector256.Shuffle(row1, Vector256.Create(2, 0, 1, 0));
                V11 = Avx.Blend(Avx2.Permute4x64(V11, 0b10_01_10_01), Avx2.Permute4x64(D0, 0b10_01_10_01), 0b1100);

                // V13 = D1Y,D1W,D2W,D2W
                var V13 = Avx.Blend(Avx2.Permute4x64(D1, 0b11_11_11_01), Avx2.Permute4x64(D2, 0b11_11_11_01), 0b1100);
                V02 = Vector256.Shuffle(row4, Vector256.Create(1, 2, 0, 1));
                V12 = Avx.Blend(Avx2.Permute4x64(V13, 0b00_11_00_10), Avx2.Permute4x64(D1, 0b00_11_00_10), 0b1100);
                var V03 = Vector256.Shuffle(row3, Vector256.Create(2, 0, 1, 0));
                V13 = Avx.Blend(Avx2.Permute4x64(V13, 0b10_01_10_01), Avx2.Permute4x64(D1, 0b10_01_10_01), 0b1100);

                var C0 = V00 * V10;
                var C2 = V01 * V11;
                var C4 = V02 * V12;
                var C6 = V03 * V13;

                // V11 = D0X,D0Y,D2X,D2X
                V11 = Avx.Blend(Avx2.Permute4x64(D0, 0b00_00_01_00), Avx2.Permute4x64(D2, 0b00_00_01_00), 0b1100);
                V00 = Vector256.Shuffle(row2, Vector256.Create(2, 3, 1, 2));
                V10 = Avx.Blend(Avx2.Permute4x64(D0, 0b10_01_00_11), Avx2.Permute4x64(V11, 0b10_01_00_11), 0b1100);
                V01 = Vector256.Shuffle(row1, Vector256.Create(3, 2, 3, 1));
                V11 = Avx.Blend(Avx2.Permute4x64(D0, 0b00_10_01_10), Avx2.Permute4x64(V11, 0b00_10_01_10), 0b1100);

                // V13 = D1X,D1Y,D2Z,D2Z
                V13 = Avx.Blend(Avx2.Permute4x64(D1, 0b10_10_01_00), Avx2.Permute4x64(D2, 0b10_10_01_00), 0b1100);
                V02 = Vector256.Shuffle(row4, Vector256.Create(2, 3, 1, 2));
                V12 = Avx.Blend(Avx2.Permute4x64(D1, 0b10_01_00_11), Avx2.Permute4x64(V13, 0b10_01_00_11), 0b1100);
                V03 = Vector256.Shuffle(row3, Vector256.Create(3, 2, 3, 1));
                V13 = Avx.Blend(Avx2.Permute4x64(D1, 0b_00_10_01_10), Avx2.Permute4x64(V13, 0b_00_10_01_10), 0b1100);

                C0 = Vector256.MultiplyAddEstimate(-V00, V10, C0);
                C2 = Vector256.MultiplyAddEstimate(-V01, V11, C2);
                C4 = Vector256.MultiplyAddEstimate(-V02, V12, C4);
                C6 = Vector256.MultiplyAddEstimate(-V03, V13, C6);

                V00 = Vector256.Shuffle(row2, Vector256.Create(3, 0, 3, 0));

                // V10 = D0Z,D0Z,D2X,D2Y
                V10 = Avx.Blend(Avx2.Permute4x64(D0, 0b01_00_10_10), Avx2.Permute4x64(D2, 0b01_00_10_10), 0b1100);
                V10 = Vector256.Shuffle(V10, Vector256.Create(0, 3, 2, 0));
                V01 = Vector256.Shuffle(row1, Vector256.Create(1, 3, 0, 2));

                // V11 = D0X,D0W,D2X,D2Y
                V11 = Avx.Blend(Avx2.Permute4x64(D0, 0b01_00_11_00), Avx2.Permute4x64(D2, 0b01_00_11_00), 0b1100);
                V11 = Vector256.Shuffle(V11, Vector256.Create(3, 0, 1, 2));
                V02 = Vector256.Shuffle(row4, Vector256.Create(3, 0, 3, 0));

                // V12 = D1Z,D1Z,D2Z,D2W
                V12 = Avx.Blend(Avx2.Permute4x64(D1, 0b11_10_10_10), Avx2.Permute4x64(D2, 0b11_10_10_10), 0b1100);
                V12 = Vector256.Shuffle(V12, Vector256.Create(0, 3, 2, 0));
                V03 = Vector256.Shuffle(row3, Vector256.Create(1, 3, 0, 2));

                // V13 = D1X,D1W,D2Z,D2W
                V13 = Avx.Blend(Avx2.Permute4x64(D1, 0b11_10_11_00), Avx2.Permute4x64(D2, 0b11_10_11_00), 0b1100);
                V13 = Vector256.Shuffle(V13, Vector256.Create(3, 0, 1, 2));

                V00 *= V10;
                V01 *= V11;
                V02 *= V12;
                V03 *= V13;

                var C1 = C0 - V00;
                C0 += V00;

                var C3 = C2 + V01;
                C2 -= V01;

                var C5 = C4 - V02;
                C4 += V02;

                var C7 = C6 + V03;
                C6 -= V03;

                C0 = Avx.Blend(Avx2.Permute4x64(C0, 0b11_01_10_00), Avx2.Permute4x64(C1, 0b11_01_10_00), 0b1100);
                C2 = Avx.Blend(Avx2.Permute4x64(C2, 0b11_01_10_00), Avx2.Permute4x64(C3, 0b11_01_10_00), 0b1100);
                C4 = Avx.Blend(Avx2.Permute4x64(C4, 0b11_01_10_00), Avx2.Permute4x64(C5, 0b11_01_10_00), 0b1100);
                C6 = Avx.Blend(Avx2.Permute4x64(C6, 0b11_01_10_00), Avx2.Permute4x64(C7, 0b11_01_10_00), 0b1100);

                C0 = Vector256.Shuffle(C0, Vector256.Create(0, 2, 1, 3));
                C2 = Vector256.Shuffle(C2, Vector256.Create(0, 2, 1, 3));
                C4 = Vector256.Shuffle(C4, Vector256.Create(0, 2, 1, 3));
                C6 = Vector256.Shuffle(C6, Vector256.Create(0, 2, 1, 3));

                // Get the determinant
                var det = Vector4D.Dot(C0.AsVector4D(), row1.AsVector4D());

                // Check determinate is not zero
                if (double.Abs(det) < double.Epsilon)
                {
                    var vNaN = Vector4D.NaN;

                    result.X = vNaN;
                    result.Y = vNaN;
                    result.Z = vNaN;
                    result.W = vNaN;

                    return false;
                }

                // Create Vector256<double> copy of the determinant and invert them.
                var vTemp = Vector256<double>.One / det;

                result.X = (C0 * vTemp).AsVector4D();
                result.Y = (C2 * vTemp).AsVector4D();
                result.Z = (C4 * vTemp).AsVector4D();
                result.W = (C6 * vTemp).AsVector4D();

                return true;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Checked")]
            static bool SoftwareFallback(in Impl matrix, out Impl result)
            {
                double a = matrix.X.X, b = matrix.X.Y, c = matrix.X.Z, d = matrix.X.W;
                double e = matrix.Y.X, f = matrix.Y.Y, g = matrix.Y.Z, h = matrix.Y.W;
                double i = matrix.Z.X, j = matrix.Z.Y, k = matrix.Z.Z, l = matrix.Z.W;
                double m = matrix.W.X, n = matrix.W.Y, o = matrix.W.Z, p = matrix.W.W;

                var kp_lo = (k * p) - (l * o);
                var jp_ln = (j * p) - (l * n);
                var jo_kn = (j * o) - (k * n);
                var ip_lm = (i * p) - (l * m);
                var io_km = (i * o) - (k * m);
                var in_jm = (i * n) - (j * m);

                var a11 = +((f * kp_lo) - (g * jp_ln) + (h * jo_kn));
                var a12 = -((e * kp_lo) - (g * ip_lm) + (h * io_km));
                var a13 = +((e * jp_ln) - (f * ip_lm) + (h * in_jm));
                var a14 = -((e * jo_kn) - (f * io_km) + (g * in_jm));

                var det = (a * a11) + (b * a12) + (c * a13) + (d * a14);

                if (double.Abs(det) < double.Epsilon)
                {
                    var vNaN = Vector4D.NaN;

                    result.X = vNaN;
                    result.Y = vNaN;
                    result.Z = vNaN;
                    result.W = vNaN;

                    return false;
                }

                var invDet = 1.0 / det;

                result.X.X = a11 * invDet;
                result.Y.X = a12 * invDet;
                result.Z.X = a13 * invDet;
                result.W.X = a14 * invDet;

                result.X.Y = -((b * kp_lo) - (c * jp_ln) + (d * jo_kn)) * invDet;
                result.Y.Y = +((a * kp_lo) - (c * ip_lm) + (d * io_km)) * invDet;
                result.Z.Y = -((a * jp_ln) - (b * ip_lm) + (d * in_jm)) * invDet;
                result.W.Y = +((a * jo_kn) - (b * io_km) + (c * in_jm)) * invDet;

                var gp_ho = (g * p) - (h * o);
                var fp_hn = (f * p) - (h * n);
                var fo_gn = (f * o) - (g * n);
                var ep_hm = (e * p) - (h * m);
                var eo_gm = (e * o) - (g * m);
                var en_fm = (e * n) - (f * m);

                result.X.Z = +((b * gp_ho) - (c * fp_hn) + (d * fo_gn)) * invDet;
                result.Y.Z = -((a * gp_ho) - (c * ep_hm) + (d * eo_gm)) * invDet;
                result.Z.Z = +((a * fp_hn) - (b * ep_hm) + (d * en_fm)) * invDet;
                result.W.Z = -((a * fo_gn) - (b * eo_gm) + (c * en_fm)) * invDet;

                var gl_hk = (g * l) - (h * k);
                var fl_hj = (f * l) - (h * j);
                var fk_gj = (f * k) - (g * j);
                var el_hi = (e * l) - (h * i);
                var ek_gi = (e * k) - (g * i);
                var ej_fi = (e * j) - (f * i);

                result.X.W = -((b * gl_hk) - (c * fl_hj) + (d * fk_gj)) * invDet;
                result.Y.W = +((a * gl_hk) - (c * el_hi) + (d * ek_gi)) * invDet;
                result.Z.W = -((a * fl_hj) - (b * el_hi) + (d * ej_fi)) * invDet;
                result.W.W = +((a * fk_gj) - (b * ek_gi) + (c * ej_fi)) * invDet;

                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl Lerp(in Impl left, in Impl right, double amount)
        {
            Impl result;

            result.X = Vector4D.Lerp(left.X, right.X, amount);
            result.Y = Vector4D.Lerp(left.Y, right.Y, amount);
            result.Z = Vector4D.Lerp(left.Z, right.Z, amount);
            result.W = Vector4D.Lerp(left.W, right.W, amount);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl Transform(in Impl value, in QuaternionD rotation)
        {
            // Compute rotation matrix.
            var x2 = rotation.X + rotation.X;
            var y2 = rotation.Y + rotation.Y;
            var z2 = rotation.Z + rotation.Z;

            var wx2 = rotation.W * x2;
            var wy2 = rotation.W * y2;
            var wz2 = rotation.W * z2;

            var xx2 = rotation.X * x2;
            var xy2 = rotation.X * y2;
            var xz2 = rotation.X * z2;

            var yy2 = rotation.Y * y2;
            var yz2 = rotation.Y * z2;
            var zz2 = rotation.Z * z2;

            var q11 = 1.0 - yy2 - zz2;
            var q21 = xy2 - wz2;
            var q31 = xz2 + wy2;

            var q12 = xy2 + wz2;
            var q22 = 1.0 - xx2 - zz2;
            var q32 = yz2 - wx2;

            var q13 = xz2 - wy2;
            var q23 = yz2 + wx2;
            var q33 = 1.0 - xx2 - yy2;

            Impl result;

            result.X = Vector4D.Create(
                (value.X.X * q11) + (value.X.Y * q21) + (value.X.Z * q31),
                (value.X.X * q12) + (value.X.Y * q22) + (value.X.Z * q32),
                (value.X.X * q13) + (value.X.Y * q23) + (value.X.Z * q33),
                value.X.W);
            result.Y = Vector4D.Create(
                (value.Y.X * q11) + (value.Y.Y * q21) + (value.Y.Z * q31),
                (value.Y.X * q12) + (value.Y.Y * q22) + (value.Y.Z * q32),
                (value.Y.X * q13) + (value.Y.Y * q23) + (value.Y.Z * q33),
                value.Y.W);
            result.Z = Vector4D.Create(
                (value.Z.X * q11) + (value.Z.Y * q21) + (value.Z.Z * q31),
                (value.Z.X * q12) + (value.Z.Y * q22) + (value.Z.Z * q32),
                (value.Z.X * q13) + (value.Z.Y * q23) + (value.Z.Z * q33),
                value.Z.W);
            result.W = Vector4D.Create(
                (value.W.X * q11) + (value.W.Y * q21) + (value.W.Z * q31),
                (value.W.X * q12) + (value.W.Y * q22) + (value.W.Z * q32),
                (value.W.X * q13) + (value.W.Y * q23) + (value.W.Z * q33),
                value.W.W);

            return result;
        }

        // This implementation is based on the DirectX Math Library XMMatrixTranspose method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMatrix.inl
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Impl Transpose(in Impl matrix)
        {
            Impl result;

            if (Avx.IsSupported)
            {
                var x = matrix.X.AsVector256();
                var y = matrix.Y.AsVector256();
                var z = matrix.Z.AsVector256();
                var w = matrix.W.AsVector256();

                var lowerXy = Avx.UnpackLow(x, y); // x[0], z[0], x[2], z[2]
                var lowerZw = Avx.UnpackLow(z, w); // y[0], w[0], y[2], w[2]
                var upperXy = Avx.UnpackHigh(x, y); // x[1], z[1], x[4], z[4]
                var upperZw = Avx.UnpackHigh(z, w); // y[1], w[1], y[4], w[4]

                result.X = Avx.InsertVector128(lowerXy, lowerZw.GetLower(), 1).AsVector4D(); // x[0], y[0], z[0], w[0]
                result.Y = Avx.InsertVector128(upperXy, upperZw.GetLower(), 1).AsVector4D(); // x[1], y[1], z[1], w[1]
                result.Z = Avx.Permute2x128(lowerXy, lowerZw, 0b_00_11_00_01).AsVector4D(); // x[2], y[2], z[2], w[2]
                result.W = Avx.Permute2x128(upperXy, upperZw, 0b_00_11_00_01).AsVector4D(); // x[3], y[3], z[3], w[3]
            }
            else
            {
                result.X = Vector4D.Create(matrix.X.X, matrix.Y.X, matrix.Z.X, matrix.W.X);
                result.Y = Vector4D.Create(matrix.X.Y, matrix.Y.Y, matrix.Z.Y, matrix.W.Y);
                result.Z = Vector4D.Create(matrix.X.Z, matrix.Y.Z, matrix.Z.Z, matrix.W.Z);
                result.W = Vector4D.Create(matrix.X.W, matrix.Y.W, matrix.Z.W, matrix.W.W);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj)
            => obj is Matrix4x4D other && this.Equals(in other.AsImpl());

        // This function needs to account for floating-point equality around NaN
        // and so must behave equivalently to the underlying double/double.Equals
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(in Impl other) =>
            this.X.Equals(other.X)
            && this.Y.Equals(other.Y)
            && this.Z.Equals(other.Z)
            && this.W.Equals(other.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Checked")]
        public readonly double GetDeterminant()
        {
            double a = this.X.X, b = this.X.Y, c = this.X.Z, d = this.X.W;
            double e = this.Y.X, f = this.Y.Y, g = this.Y.Z, h = this.Y.W;
            double i = this.Z.X, j = this.Z.Y, k = this.Z.Z, l = this.Z.W;
            double m = this.W.X, n = this.W.Y, o = this.W.Z, p = this.W.W;

            var kp_lo = (k * p) - (l * o);
            var jp_ln = (j * p) - (l * n);
            var jo_kn = (j * o) - (k * n);
            var ip_lm = (i * p) - (l * m);
            var io_km = (i * o) - (k * m);
            var in_jm = (i * n) - (j * m);

            return (a * ((f * kp_lo) - (g * jp_ln) + (h * jo_kn))) -
                   (b * ((e * kp_lo) - (g * ip_lm) + (h * io_km))) +
                   (c * ((e * jp_ln) - (f * ip_lm) + (h * in_jm))) -
                   (d * ((e * jo_kn) - (f * io_km) + (g * in_jm)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.Combine(this.X, this.Y, this.Z, this.W);

        [System.Diagnostics.CodeAnalysis.UnscopedRef]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0102:Make member readonly", Justification = "Checked")]
        public ref Matrix4x4D AsM4x4D() => ref Unsafe.As<Impl, Matrix4x4D>(ref this);

        readonly bool IEquatable<Impl>.Equals(Impl other) => this.Equals(in other);

        [System.Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowPlatformNotSupportedException() => throw new PlatformNotSupportedException();
    }
}