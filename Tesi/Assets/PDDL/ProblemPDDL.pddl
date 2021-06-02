(define 
(problem S_Example_P)
(:domain S_Example)
(:objects 
c1 - collector
p1 - producer
w1 - wood
st1 - stone
s1 - storage
rs1 - r_station
)
(:init 
(= (battery-amount_c1) 15)
(= (battery-amount_p1) 15)
(= (wood-amount_c1) 0)
(= (stone-amount_c1) 0)
(= (wood-amount_p1) 0)
(= (stone-amount_p1) 0)
(= (chest-amount_p1) 0)
(= (posX_c1) 0)
(= (posY_c1) 0)
(= (posX_p1) 1)
(= (posY_p1) 2)
(free_p1)
(free_c1)
(= (wood-stored_s1) 0)
(= (stone-stored_s1) 0)
(= (chest-stored_s1) 0)
(= (posX_w1) 1)
(= (posY_w1) 1)
(= (posX_st1) 1)
(= (posY_st1) 1)
(= (posX_s1) 1)
(= (posY_s1) 1)
(= (posX_rs1) 1)
(= (posY_rs1) 1)
(= (battery-capacity) 100)
(= (sample-capacity) 100)
)
(:goal 
(and 
(= (wood-stored_s1) 1)
(= (stone-stored_s1) 1)
)
)
)
