(define 
(problem Test)
(:domain S_Example)
(:objects 
w1 - wood
w2 - wood
s1 - stone
s2 - stone
st - storage
c1 - collector
p1 - producer
rs1 - r-station
)
(:init 
(= (posX w1) 3)
(= (posY w1) 3)
(= (posX w2) 9)
(= (posY w2) 9)
(= (posX s1) 3)
(= (posY s1) 9)
(= (posX s2) 9)
(= (posY s2) 3)
(= (posX st) 6)
(= (posY st) 6)
(= (wood-stored st) 0)
(= (stone-stored st) 0)
(= (chest-stored st) 0)
(= (battery-amount c1) 75)
(= (posX c1) 2)
(= (posY c1) 3)
(= (wood-amount c1) 0)
(= (stone-amount c1) 0)
(free c1)
(= (battery-amount p1) 50)
(= (posX p1) 8)
(= (posY p1) 7)
(= (wood-amount p1) 0)
(= (stone-amount p1) 0)
(free p1)
(= (posX rs1) 0)
(= (posY rs1) 0)
(= (battery-capacity) 100)
(= (sample-capacity) 15)
(= (grid-size) 11)
)
(:goal 
(and 
(= (wood-stored st) 3)
(= (stone-stored st) 8)
)
)
)
