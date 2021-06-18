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
(= (battery-amount c1) 50)
(= (battery-amount p1) 50)
(= (wood-amount c1) 0)
(= (stone-amount c1) 0)
(= (wood-amount p1) 0)
(= (stone-amount p1) 0)
(= (chest-amount p1) 0)
(= (posX c1) 6)
(= (posY c1) 4)
(= (posX p1) 1)
(= (posY p1) 2)
(free p1)
(free c1)
(= (wood-stored s1) 0)
(= (stone-stored s1) 0)
(= (chest-stored s1) 0)
(= (posX w1) 1)
(= (posY w1) 1)
(= (posX st1) 4)
(= (posY st1) 2)
(= (posX s1) 7)
(= (posY s1) 8)
(= (posX rs1) 9)
(= (posY rs1) 2)
(= (battery-capacity) 100)
(= (sample-capacity) 100)
)
(:goal 
(and 
(= (wood-stored s1) 2)
(= (stone-stored s1) 3)
)
)
)
