(define (domain S_Example)
(:requirements :strips :durative-actions :fluents :duration-inequalities :typing :continuous-effects )
(:types
robot r_station resource storage - object
collector producer - robot
wood stone - resource
)
(:predicates 
(free_r)
)
(:functions 
(battery-amount_r)
(wood-amount_r)
(stone-amount_r)
(chest-amount_p)
(posX_o)
(posY_o)
(wood-stored_s)
(stone-stored_s)
(chest-stored_s)
(battery-capacity)
(sample-capacity)
)
(:durative-action move-up
:parameters (?r - robot )
:duration (= ?duration 3)
:condition (and
(at start (> (battery-amount_r) 10))
(at start (free_r))
)
:effect (and
(at start (not (free_r)))
(at end (free_r))
(at end (decrease (battery-amount_r) 2))
(at end (increase (posY_r) 1))
)
)(:durative-action move-down
:parameters (?r - robot )
:duration (= ?duration 3)
:condition (and
(at start (> (battery-amount_r) 10))
(at start (free_r))
)
:effect (and
(at start (not (free_r)))
(at end (free_r))
(at end (decrease (battery-amount_r) 2))
(at end (decrease (posY_r) 1))
)
)(:durative-action move-right
:parameters (?r - robot )
:duration (= ?duration 3)
:condition (and
(at start (> (battery-amount_r) 10))
(at start (free_r))
)
:effect (and
(at start (not (free_r)))
(at end (free_r))
(at end (decrease (battery-amount_r) 2))
(at end (increase (posX_r) 1))
)
)(:durative-action move-left
:parameters (?r - robot )
:duration (= ?duration 3)
:condition (and
(at start (> (battery-amount_r) 10))
(at start (free_r))
)
:effect (and
(at start (not (free_r)))
(at end (free_r))
(at end (decrease (battery-amount_r) 2))
(at end (decrease (posX_r) 1))
)
)(:durative-action collect-wood
:parameters (?c - collector ?w - wood )
:duration (= ?duration 5)
:condition (and
(at start (> (battery-amount_c) 10))
(at start (= (posX_c) (posX_w)))
(at start (= (posY_c) (posY_w)))
(at start (< (wood-amount_c) (sample-capacity)))
(at start (free_c))
)
:effect (and
(at start (not (free_c)))
(at end (free_c))
(at end (decrease (battery-amount_c) 5))
(at end (increase (wood-amount_c) 1))
)
)(:durative-action collect-stone
:parameters (?c - collector ?s - stone )
:duration (= ?duration 5)
:condition (and
(at start (> (battery-amount_c) 10))
(at start (= (posX_c) (posX_s)))
(at start (= (posY_c) (posY_s)))
(at start (< (stone-amount_c) (sample-capacity)))
(at start (free_c))
)
:effect (and
(at start (not (free_c)))
(at end (free_c))
(at end (decrease (battery-amount_c) 5))
(at end (increase (stone-amount_c) 1))
)
)(:durative-action recharge
:parameters (?r - robot ?rs - r_station )
:duration (= ?duration (- (battery-capacity) (battery-amount_r)))
:condition (and
(at start (> (battery-amount_r) 0))
(at start (= (posX_r) (posX_rs)))
(at start (= (posY_r) (posY_rs)))
(at start (< (battery-amount_r) (battery-capacity)))
(at start (free_r))
)
:effect (and
(at start (not (free_r)))
(at end (free_r))
(at end (increase (battery-amount_r) (- (battery-capacity) (battery-amount_r))))
)
)(:durative-action store-wood
:parameters (?r - robot ?s - storage )
:duration (= ?duration 1)
:condition (and
(at start (> (battery-amount_r) 10))
(at start (> (wood-amount_r) 0))
(at start (= (posX_r) (posX_s)))
(at start (= (posY_r) (posY_s)))
(at start (free_r))
)
:effect (and
(at start (not (free_r)))
(at end (free_r))
(at end (decrease (battery-amount_r) 1))
(at end (increase (wood-stored_s) 1))
(at end (decrease (wood-amount_r) 1))
)
)(:durative-action store-stone
:parameters (?r - robot ?s - storage )
:duration (= ?duration 1)
:condition (and
(at start (> (battery-amount_r) 10))
(at start (> (stone-amount_r) 0))
(at start (= (posX_r) (posX_s)))
(at start (= (posY_r) (posY_s)))
(at start (free_r))
)
:effect (and
(at start (not (free_r)))
(at end (free_r))
(at end (decrease (battery-amount_r) 1))
(at end (increase (stone-stored_s) 1))
(at end (decrease (stone-amount_r) 1))
)
)(:durative-action store-chest
:parameters (?p - producer ?s - storage )
:duration (= ?duration 1)
:condition (and
(at start (> (battery-amount_p) 10))
(at start (> (chest-amount_p) 0))
(at start (= (posX_p) (posX_s)))
(at start (= (posY_p) (posY_s)))
(at start (free_p))
)
:effect (and
(at start (not (free_p)))
(at end (free_p))
(at end (decrease (battery-amount_p) 1))
(at end (increase (chest-stored_s) 1))
(at end (decrease (chest-amount_p) 1))
)
)(:durative-action retrieve-wood
:parameters (?p - producer ?c - collector )
:duration (= ?duration 2)
:condition (and
(at start (= (posX_p) (posX_c)))
(at start (= (posY_p) (posY_c)))
(at start (> (wood-amount_c) 0))
(at start (free_c))
(at start (free_p))
)
:effect (and
(at start (not (free_c)))
(at start (not (free_p)))
(at end (free_c))
(at end (free_p))
(at end (decrease (wood-amount_c) 1))
(at end (increase (wood-amount_p) 1))
)
)(:durative-action retrieve-stone
:parameters (?p - producer ?c - collector )
:duration (= ?duration 2)
:condition (and
(at start (= (posX_p) (posX_c)))
(at start (= (posY_p) (posY_c)))
(at start (> (stone-amount_c) 0))
(at start (free_c))
(at start (free_p))
)
:effect (and
(at start (not (free_c)))
(at start (not (free_p)))
(at end (free_c))
(at end (free_p))
(at end (decrease (stone-amount_c) 1))
(at end (increase (stone-amount_p) 1))
)
)(:durative-action produce-chest
:parameters (?p - producer )
:duration (= ?duration 5)
:condition (and
(at start (> (battery-amount_p) 10))
(at start (> (wood-amount_p) 0))
(at start (> (stone-amount_p) 0))
(at start (free_p))
)
:effect (and
(at start (not (free_p)))
(at end (free_p))
(at end (decrease (battery-amount_p) 5))
(at end (increase (chest-amount_p) 1))
(at end (decrease (wood-amount_p) 1))
(at end (decrease (stone-amount_p) 1))
)
))

