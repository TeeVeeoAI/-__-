#[repr(C)]
pub struct Rect {
    pub x: f32,
    pub y: f32,
    pub w: f32,
    pub h: f32,
}

#[repr(C)]
pub struct Circle {
    pub x: f32,
    pub y: f32,
    pub radius: f32,
}

/// Returns 1 if the two AABBs overlap, 0 otherwise
#[no_mangle]
pub extern "C" fn aabb_overlaps(a: Rect, b: Rect) -> i32 {
    let overlap = a.x < b.x + b.w
        && a.x + a.w > b.x
        && a.y < b.y + b.h
        && a.y + a.h > b.y;
    overlap as i32
}

/// Returns 1 if a point is inside a rect
#[no_mangle]
pub extern "C" fn point_in_rect(px: f32, py: f32, r: Rect) -> i32 {
    (px >= r.x && px <= r.x + r.w && py >= r.y && py <= r.y + r.h) as i32
}

/// Returns 1 if two circles overlap
#[no_mangle]
pub extern "C" fn circles_overlap(a: Circle, b: Circle) -> i32 {
    let dx = a.x - b.x;
    let dy = a.y - b.y;
    let dist_sq = dx * dx + dy * dy;
    let r_sum = a.radius + b.radius;
    (dist_sq <= r_sum * r_sum) as i32
}